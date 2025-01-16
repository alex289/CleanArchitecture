using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Tenants.GetTenantById;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Tenants;
using CleanArchitecture.Domain.Errors;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Application.Tests.Queries.Tenants;

public sealed class GetTenantByIdQueryHandlerTests
{
    private readonly GetTenantByIdTestFixture _fixture = new();

    [Fact]
    public async Task Should_Get_Existing_Tenant()
    {
        var tenant = _fixture.SetupTenant();

        var result = await _fixture.QueryHandler.Handle(
            new GetTenantByIdQuery(tenant.Id),
            default);

        _fixture.VerifyNoDomainNotification();

        tenant.Id.ShouldBe(result!.Id);
        tenant.Name.ShouldBe(result.Name);
        tenant.Users.Count.ShouldBe(result.Users.Count());
    }

    [Fact]
    public async Task Should_Not_Get_Deleted_Tenant()
    {
        var tenant = _fixture.SetupTenant(true);

        var result = await _fixture.QueryHandler.Handle(
            new GetTenantByIdQuery(tenant.Id),
            default);

        _fixture.VerifyExistingNotification(
            nameof(GetTenantByIdQuery),
            ErrorCodes.ObjectNotFound,
            $"Tenant with id {tenant.Id} could not be found");
        result.ShouldBeNull();
    }
}