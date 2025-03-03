using Xunit;

namespace Microblog.EntityFrameworkCore;

[CollectionDefinition(MicroblogTestConsts.CollectionDefinitionName)]
public class MicroblogEntityFrameworkCoreCollection : ICollectionFixture<MicroblogEntityFrameworkCoreFixture>
{

}
