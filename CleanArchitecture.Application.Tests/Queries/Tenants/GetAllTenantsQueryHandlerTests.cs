using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Tenants.GetAll;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Tenants;
using CleanArchitecture.Application.ViewModels;
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

        var query = new PageQuery
        {
            PageSize = 10,
            Page = 1
        };

        var result = await _fixture.QueryHandler.Handle(
            new GetAllTenantsQuery(query, false),
            default);

        _fixture.VerifyNoDomainNotification();

        result.PageSize.Should().Be(query.PageSize);
        result.Page.Should().Be(query.Page);
        result.Count.Should().Be(1);

        tenant.Should().BeEquivalentTo(result.Items.First());
    }

    [Fact]
    public async Task Should_Not_Get_Deleted_Tenant()
    {
        _fixture.SetupTenant(true);

        var query = new PageQuery
        {
            PageSize = 10,
            Page = 1
        };

        var result = await _fixture.QueryHandler.Handle(
            new GetAllTenantsQuery(query, false),
            default);

        result.PageSize.Should().Be(query.PageSize);
        result.Page.Should().Be(query.Page);
        result.Count.Should().Be(0);

        result.Items.Should().HaveCount(0);
    }
}