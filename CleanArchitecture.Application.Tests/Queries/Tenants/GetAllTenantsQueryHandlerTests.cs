using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Tenants.GetAll;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Tenants;
using CleanArchitecture.Application.ViewModels;
using Shouldly;
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

        result.PageSize.ShouldBe(query.PageSize);
        result.Page.ShouldBe(query.Page);
        result.Count.ShouldBe(1);

        var checkingTenant = result.Items.First();

        tenant.Id.ShouldBe(checkingTenant.Id);
        tenant.Name.ShouldBe(checkingTenant.Name);
        tenant.Users.Count.ShouldBe(checkingTenant.Users.Count());
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

        result.PageSize.ShouldBe(query.PageSize);
        result.Page.ShouldBe(query.Page);
        result.Count.ShouldBe(0);

        result.Items.Count.ShouldBe(0);
    }
}