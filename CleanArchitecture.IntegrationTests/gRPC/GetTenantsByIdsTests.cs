using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures.gRPC;
using CleanArchitecture.Proto.Tenants;
using Shouldly;

namespace CleanArchitecture.IntegrationTests.gRPC;

public sealed class GetTenantsByIdsTests
{
    private readonly GetTenantsByIdsTestFixture _fixture = new();

    [OneTimeSetUp]
    public async Task Setup() => await _fixture.SeedTestData();

    [Test]
    public async Task Should_Get_Tenants_By_Ids()
    {
        var client = new TenantsApi.TenantsApiClient(_fixture.GrpcChannel);

        var request = new GetTenantsByIdsRequest();
        request.Ids.Add(_fixture.CreatedTenantId.ToString());

        var response = await client.GetByIdsAsync(request);

        response.Tenants.ShouldHaveSingleItem();

        var tenant = response.Tenants.First();
        var createdTenant = _fixture.CreateTenant();

        new Guid(tenant.Id).ShouldBe(createdTenant.Id);
        tenant.Name.ShouldBe(createdTenant.Name);
        tenant.DeletedAt.ShouldNotBeNullOrWhiteSpace();
    }
}