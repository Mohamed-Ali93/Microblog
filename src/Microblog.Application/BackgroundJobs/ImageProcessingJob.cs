using Microblog.Posts;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Microblog.BackgroundJobs
{
    public class ImageProcessingJobArgs
    {
        public Guid PostId { get; set; }
        public string OriginalImageUrl { get; set; }
    }

    public class PostImageBlob { } // Marker interface for post images

    public class ImageProcessingJob : AsyncBackgroundJob<ImageProcessingJobArgs>, ITransientDependency
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IBlobContainer _blobContainer;
        private readonly ILogger<ImageProcessingJob> _logger;
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        // Define standard image dimensions to resize to
        private readonly List<(int width, int height)> _standardDimensions = new List<(int, int)>
        {
            (1200, 675),  // 16:9 large
            (800, 450),   // 16:9 medium
            (600, 600),   // 1:1 square
            (400, 300),   // 4:3 small
            (320, 568)    // Mobile portrait
        };

        public ImageProcessingJob(
            IRepository<Post, Guid> postRepository,
            IBlobContainer blobContainer,
            ILogger<ImageProcessingJob> logger,
            IUnitOfWorkManager unitOfWorkManager,
            IHttpClientFactory httpClientFactory)
        {
            _postRepository = postRepository;
            _blobContainer = blobContainer;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
            _httpClient = httpClientFactory.CreateClient("ImageProcessing");
        }

        public override async Task ExecuteAsync(ImageProcessingJobArgs args)
        {
            _logger.LogInformation("start processing image for post {PostId}", args.PostId);
            using (var uow  = _unitOfWorkManager.Begin())
            {
                try
                {

                    // Get the post
                    var post = await _postRepository.GetAsync(args.PostId);

                    // Download the original image
                    var imageData = await _blobContainer.GetAllBytesAsync(args.OriginalImageUrl);

                    // Process the image for each standard dimension
                    var processedImageUrls = new List<ProcessedImage>();
                    foreach (var dimension in _standardDimensions)
                    {
                        var processedImageStream = await ProcessImageAsync(
                            imageData,
                            dimension.width,
                            dimension.height
                        );

                        // Generate unique blob name
                        var blobName = $"{post.Id}_{dimension.width}x{dimension.height}.webp";

                        // Save processed image to blob container
                        var remoteStreamContent = new RemoteStreamContent(
                            processedImageStream,
                            blobName,
                            "image/webp"
                        );

                        await _blobContainer.SaveAsync(blobName, processedImageStream);

                        // Add the processed image to the post
                        processedImageUrls.Add(new ProcessedImage(
                            Guid.NewGuid(),
                            dimension.width,
                            dimension.height,
                            blobName,
                            post.Id
                        ));
                    }

                    // Update the post with processed images
                    post.ProcessedImages.AddRange(processedImageUrls);
                    await _postRepository.UpdateAsync(post, true);
                    await uow.CompleteAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing image for post {PostId}", args.PostId);
                    throw;
                }
            }
            
        }

        private async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            var response = await _httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        private async Task<Stream> ProcessImageAsync(byte[] imageData, int width, int height)
        {
            var memoryStream = new MemoryStream();

            // Load the image using ImageSharp
            using (var image = Image.Load(imageData))
            {
                // Resize the image while maintaining aspect ratio
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));

                // Convert to WebP and save to memory stream
                await image.SaveAsWebpAsync(memoryStream, new WebpEncoder
                {
                    Quality = 80,
                    FileFormat = WebpFileFormatType.Lossy
                });
            }

            // Reset memory stream position
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}