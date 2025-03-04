using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Microblog.Posts
{

    public class PostDto : AuditedEntityDto<Guid>
    {
        public string Content { get; set; }
        public string Username { get; set; }
        public string OriginalImageUrl { get; set; }
        public bool HasImage { get; set; }
        public bool IsImageProcessed { get; set; }
        public GeoCoordinateDto Location { get; set; }
        public ProcessedImageDto BestMatchImage { get; set; }
    }
}