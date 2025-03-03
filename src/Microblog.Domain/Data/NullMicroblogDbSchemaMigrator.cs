using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Microblog.Data;

/* This is used if database provider does't define
 * IMicroblogDbSchemaMigrator implementation.
 */
public class NullMicroblogDbSchemaMigrator : IMicroblogDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
