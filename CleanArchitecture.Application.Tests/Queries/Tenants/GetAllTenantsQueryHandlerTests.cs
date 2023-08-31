using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Tenants.GetAll;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Tenants;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Application.Tests.Queries.Tenants;

public sealed class GetAllTenantsQueryHandlerTests
{
    private readonly GetAllTenantsTestFixture _fixture = new();

    [Fact]
    public async Task Should_Get_Existing_Tenant()
    {
        var tenant = _fixture.SetupTenant();

        var result = await _fixture.QueryHandler.Handle(
            new GetAllTenantsQuery(),
            default);

        _fixture.VerifyNoDomainNotification();

        tenant.Should().BeEquivalentTo(result.First());
    }

    [Fact]
    public async Task Should_Not_Get_Deleted_Tenant()
    {
        _fixture.SetupTenant(true);

        var result = await _fixture.QueryHandler.Handle(
            new GetAllTenantsQuery(),
            default);

        result.Should().HaveCount(0);
    }
}