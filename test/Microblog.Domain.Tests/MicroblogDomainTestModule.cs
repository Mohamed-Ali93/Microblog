using Volo.Abp.Modularity;

namespace Microblog;

[DependsOn(
    typeof(MicroblogDomainModule),
    typeof(MicroblogTestBaseModule)
)]
public class MicroblogDomainTestModule : AbpModule
{

}
