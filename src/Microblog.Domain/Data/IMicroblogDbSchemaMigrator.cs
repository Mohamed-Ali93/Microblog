using System.Threading.Tasks;

namespace Microblog.Data;

public interface IMicroblogDbSchemaMigrator
{
    Task MigrateAsync();
}
