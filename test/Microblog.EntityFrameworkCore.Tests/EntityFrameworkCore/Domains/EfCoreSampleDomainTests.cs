using Microblog.Samples;
using Xunit;

namespace Microblog.EntityFrameworkCore.Domains;

[Collection(MicroblogTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MicroblogEntityFrameworkCoreTestModule>
{

}
