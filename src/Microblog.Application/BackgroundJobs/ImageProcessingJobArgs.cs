using System;

namespace Microblog.BackgroundJobs
{
    public class ImageProcessingJobArgs
    {
        public Guid PostId { get; set; }
        public string OriginalImageUrl { get; set; }
    }
}