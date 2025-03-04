using System;
using Volo.Abp.Domain.Entities;

namespace Microblog.Posts
{
    public class ProcessedImage : Entity<Guid>
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public string Url { get; protected set; }
        public Guid PostId { get; protected set; }

        protected ProcessedImage() { }

        public ProcessedImage(
            Guid id,
            int width,
            int height,
            string url,
            Guid postId) : base(id)
        {
            Width = width;
            Height = height;
            Url = url;
            PostId = postId;
        }
    }
}