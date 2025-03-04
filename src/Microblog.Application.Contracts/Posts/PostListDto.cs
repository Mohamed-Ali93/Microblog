using Volo.Abp.Application.Dtos;

namespace Microblog.Posts
{
    public class PostListDto : PagedAndSortedResultRequestDto
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
    }
}