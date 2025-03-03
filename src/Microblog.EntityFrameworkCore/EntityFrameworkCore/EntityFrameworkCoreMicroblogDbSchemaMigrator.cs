using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microblog.Data;
using Volo.Abp.DependencyInjection;

namespace Microblog.EntityFrameworkCore;

public class EntityFrameworkCoreMicroblogDbSchemaMigrator
    : IMicroblogDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMicroblogDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the MicroblogDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MicroblogDbContext>()
            .Database
            .MigrateAsync();
    }
}
