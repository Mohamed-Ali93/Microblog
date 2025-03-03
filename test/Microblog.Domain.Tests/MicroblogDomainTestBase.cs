using Volo.Abp.Modularity;

namespace Microblog;

/* Inherit from this class for your domain layer tests. */
public abstract class MicroblogDomainTestBase<TStartupModule> : MicroblogTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
