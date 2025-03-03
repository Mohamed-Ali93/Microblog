using Microblog.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Microblog.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MicroblogEntityFrameworkCoreModule),
    typeof(MicroblogApplicationContractsModule)
)]
public class MicroblogDbMigratorModule : AbpModule
{
}
