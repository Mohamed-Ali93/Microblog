using Microblog.Localization;
using Volo.Abp.Application.Services;

namespace Microblog;

/* Inherit your application services from this class.
 */
public abstract class MicroblogAppService : ApplicationService
{
    protected MicroblogAppService()
    {
        LocalizationResource = typeof(MicroblogResource);
    }
}
