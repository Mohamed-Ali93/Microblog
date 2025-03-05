using Microblog.BackgroundJobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Microblog.Posts
{
    [Authorize]
    public class PostAppService : ApplicationService, IPostAppService
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IIdentityUserRepository _userRepository;
        //private readonly IBlobStorageService _blobStorageService;
        private readonly IBlobContainer _blobStorageService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ICurrentUser _currentUser;

        public PostAppService(
            IRepository<Post, Guid> postRepository,
            IIdentityUserRepository userRepository,
            IBlobContainer blobStorageService,
            IBackgroundJobManager backgroundJobManager,
            ICurrentUser currentUser)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _blobStorageService = blobStorageService;
            _backgroundJobManager = backgroundJobManager;
            _currentUser = currentUser;
        }

        public async Task<PostDto> CreateAsync([FromForm]CreatePostDto input)
        {
            // Note: Input validation is handled by ABP validation system

            string originalImageUrl = null;

            // Upload image if provided
            if (input.Image != null && input.Image.Length > 0)
            {
                // Upload original image to blob storage
                //originalImageUrl = await _blobStorageService.UploadFileAsync(input.Image);
                using var stream = input.Image.OpenReadStream();

                // Generate unique file name
                 originalImageUrl = $"{Guid.NewGuid()}{Path.GetExtension(input.Image.FileName)}";

                await _blobStorageService.SaveAsync(
                    originalImageUrl,
                    stream
                );
            }

            // Generate random geo coordinates
            var random = new Random();
            var latitude = random.NextDouble() * 180 - 90; // -90 to 90
            var longitude = random.NextDouble() * 360 - 180; // -180 to 180
            var location = new GeoCoordinate(latitude, longitude);

            // Create post entity
            var post = new Post(
                GuidGenerator.Create(),
                input.Content,
                originalImageUrl,
                location,
                _currentUser.GetId()
            );

            // Save post
            await _postRepository.InsertAsync(post);

            // Process image in background if an image is provided
            if (originalImageUrl != null)
            {
                try
                {
                    await _backgroundJobManager.EnqueueAsync(
                    new ImageProcessingJobArgs
                    {
                        PostId = post.Id,
                        OriginalImageUrl = originalImageUrl
                    }
                );
                }
                catch (Exception ex)
                {

                    throw;
                }
                
            }

            // Map to DTO and return
            return await MapToPostDtoAsync(post);
        }
        public async Task<PagedResultDto<PostDto>> GetTimelineAsync(PostListDto input)
        {
            // Get posts ordered by creation time (descending)
            var query = await _postRepository.WithDetailsAsync(i=>i.ProcessedImages);
            var posts = query
                .OrderByDescending(p => p.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var totalCount = await _postRepository.GetCountAsync();

            // Map to DTOs
            var postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                postDtos.Add(await MapToPostDtoAsync(post, input.ScreenWidth, input.ScreenHeight));
            }

            return new PagedResultDto<PostDto>
            {
                TotalCount = totalCount,
                Items = postDtos
            };
        }
        [HttpGet]
        public async Task<IActionResult> GetImageAsync(string blobName)
        {
            try
            {
                var blob = await _blobStorageService.GetAsync(blobName);
                var memoryStream = new MemoryStream();
                await blob.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return new FileStreamResult(memoryStream, "image/webp");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<PostDto> MapToPostDtoAsync(Post post, int screenWidth = 0, int screenHeight = 0)
        {
            var user = await _userRepository.GetAsync(post.UserId);

            var dto = new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                Username = user.UserName,
                OriginalImageUrl = post.OriginalImageUrl,
                HasImage = post.OriginalImageUrl != null,
                IsImageProcessed = post.IsImageProcessed,
                CreationTime = post.CreationTime,
                LastModificationTime = post.LastModificationTime,
                Location = new GeoCoordinateDto
                {
                    Latitude = post.Location.Latitude,
                    Longitude = post.Location.Longitude
                }
            };

            // Find best matching image based on screen dimensions
            if (post.IsImageProcessed && screenWidth > 0 && screenHeight > 0)
            {
                var bestMatch = post.ProcessedImages
                    .OrderBy(img => Math.Abs((double)img.Width / img.Height - (double)screenWidth / screenHeight))
                    .FirstOrDefault();

                if (bestMatch != null)
                {
                    dto.BestMatchImage = new ProcessedImageDto
                    {
                        Id = bestMatch.Id,
                        Width = bestMatch.Width,
                        Height = bestMatch.Height,
                        Url = bestMatch.Url
                    };
                }
            }

            return dto;
        }
    }
}
