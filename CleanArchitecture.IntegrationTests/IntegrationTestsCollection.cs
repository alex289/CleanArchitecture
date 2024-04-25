using CleanArchitecture.IntegrationTests.Fixtures;
using Xunit;

namespace CleanArchitecture.IntegrationTests;

[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public sealed class IntegrationTestsCollection :
    ICollectionFixture<AccessorFixture>
{
}