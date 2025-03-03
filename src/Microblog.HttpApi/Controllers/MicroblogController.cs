using Microblog.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Microblog.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MicroblogController : AbpControllerBase
{
    protected MicroblogController()
    {
        LocalizationResource = typeof(MicroblogResource);
    }
}
