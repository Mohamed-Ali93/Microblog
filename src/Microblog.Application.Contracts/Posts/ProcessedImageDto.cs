using System;

namespace Microblog.Posts
{
    public class ProcessedImageDto
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
    }
}