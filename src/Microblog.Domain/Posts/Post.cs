using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Microblog.Posts
{
    public class Post : AuditedAggregateRoot<Guid>
    {
        public  string Content { get; set; }
        public string? OriginalImageUrl { get; protected set; }
        public bool IsImageProcessed { get; protected set; }
        public List<ProcessedImage> ProcessedImages { get; protected set; }
        public  GeoCoordinate Location { get; set; }
        public Guid UserId { get; set; }

        protected Post()
        {
            ProcessedImages = new List<ProcessedImage>();
        }

        public Post(
            Guid id,
            string content,
            string originalImageUrl,
            GeoCoordinate location,
            Guid userId) : base(id)
        {
            SetContent(content);
            OriginalImageUrl = originalImageUrl;
            Location = location;
            UserId = userId;
            IsImageProcessed = false;
            ProcessedImages = new List<ProcessedImage>();
        }

        public void SetContent(string content)
        {
            if (content?.Length > 140)
            {
                throw new ArgumentException("Post content cannot exceed 140 characters.", nameof(content));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Post content required.", nameof(content));
            }

            Content = content;
        }

        public void AddProcessedImage(ProcessedImage processedImage)
        {
            ProcessedImages.Add(processedImage);

            if (ProcessedImages.Count > 0)
            {
                IsImageProcessed = true;
            }
        }
    }
}