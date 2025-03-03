using Microsoft.Extensions.Localization;
using Microblog.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Microblog;

[Dependency(ReplaceServices = true)]
public class MicroblogBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MicroblogResource> _localizer;

    public MicroblogBrandingProvider(IStringLocalizer<MicroblogResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
