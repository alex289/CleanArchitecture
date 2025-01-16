using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.gRPC.Tests.Fixtures;
using CleanArchitecture.Proto.Tenants;
using Shouldly;
using Xunit;

namespace CleanArchitecture.gRPC.Tests.Tenants;

public sealed class GetTenantsByIdsTests : IClassFixture<TenantTestFixture>
{
    private readonly TenantTestFixture _fixture;

    public GetTenantsByIdsTests(TenantTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_Empty_List_If_No_Ids_Are_Given()
    {
        var result = await _fixture.TenantsApiImplementation.GetByIds(
            SetupRequest(Enumerable.Empty<Guid>()),
            default!);

        result.Tenants.Count.ShouldBe(0);
    }

    [Fact]
    public async Task? Should_Get_Requested_Tenants()
    {
        var nonExistingId = Guid.NewGuid();

        var ids = _fixture.ExistingTenants
            .Take(2)
            .Select(tenant => tenant.Id)
            .ToList();

        ids.Add(nonExistingId);

        var result = await _fixture.TenantsApiImplementation.GetByIds(
            SetupRequest(ids),
            default!);

        result.Tenants.Count.ShouldBe(2);

        foreach (var tenant in result.Tenants)
        {
            var tenantId = Guid.Parse(tenant.Id);

            tenantId.ShouldNotBe(nonExistingId);

            var mockTenant = _fixture.ExistingTenants.First(t => t.Id == tenantId);

            mockTenant.ShouldNotBeNull();

            tenant.Name.ShouldBe(mockTenant.Name);
        }
    }

    private static GetTenantsByIdsRequest SetupRequest(IEnumerable<Guid> ids)
    {
        var request = new GetTenantsByIdsRequest();

        request.Ids.AddRange(ids.Select(id => id.ToString()));
        request.Ids.Add("Not a guid");

        return request;
    }
}