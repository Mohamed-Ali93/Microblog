using Volo.Abp.Modularity;

namespace Microblog;

[DependsOn(
    typeof(MicroblogApplicationModule),
    typeof(MicroblogDomainTestModule)
)]
public class MicroblogApplicationTestModule : AbpModule
{

}
