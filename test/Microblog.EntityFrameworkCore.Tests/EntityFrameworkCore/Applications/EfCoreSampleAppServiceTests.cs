using Microblog.Samples;
using Xunit;

namespace Microblog.EntityFrameworkCore.Applications;

[Collection(MicroblogTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MicroblogEntityFrameworkCoreTestModule>
{

}
