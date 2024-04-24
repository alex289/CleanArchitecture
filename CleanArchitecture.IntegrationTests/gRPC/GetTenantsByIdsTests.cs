using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures.gRPC;
using CleanArchitecture.Proto.Tenants;
using FluentAssertions;
using Xunit;
using Xunit.Priority;

namespace CleanArchitecture.IntegrationTests.gRPC;

[Collection("IntegrationTests")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public sealed class GetTenantsByIdsTests : IClassFixture<GetTenantsByIdsTestFixture>
{
    private readonly GetTenantsByIdsTestFixture _fixture;

    public GetTenantsByIdsTests(GetTenantsByIdsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_Tenants_By_Ids()
    {
        var client = new TenantsApi.TenantsApiClient(_fixture.GrpcChannel);

        var request = new GetTenantsByIdsRequest();
        request.Ids.Add(_fixture.CreatedTenantId.ToString());

        var response = await client.GetByIdsAsync(request);

        response.Tenants.Should().HaveCount(1);

        var tenant = response.Tenants.First();
        var createdTenant = _fixture.CreateTenant();

        new Guid(tenant.Id).Should().Be(createdTenant.Id);
        tenant.Name.Should().Be(createdTenant.Name);
    }
}