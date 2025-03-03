using Volo.Abp.Modularity;

namespace Microblog;

public abstract class MicroblogApplicationTestBase<TStartupModule> : MicroblogTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
