using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Microblog.Posts
{
    public interface IPostAppService
    {
        Task<PostDto> CreateAsync(CreatePostDto input);
        Task<PagedResultDto<PostDto>> GetTimelineAsync(PostListDto input);
    }
}
