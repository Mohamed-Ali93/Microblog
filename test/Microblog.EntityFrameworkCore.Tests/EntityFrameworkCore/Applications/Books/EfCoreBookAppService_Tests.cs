using Microblog.Books;
using Xunit;

namespace Microblog.EntityFrameworkCore.Applications.Books;

[Collection(MicroblogTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<MicroblogEntityFrameworkCoreTestModule>
{

}