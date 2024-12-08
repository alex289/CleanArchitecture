using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.gRPC.Interfaces;
using CleanArchitecture.Proto.Tenants;
using CleanArchitecture.Shared.Tenants;

namespace CleanArchitecture.gRPC.Contexts;

public sealed class TenantsContext : ITenantsContext
{
    private readonly TenantsApi.TenantsApiClient _client;

    public TenantsContext(TenantsApi.TenantsApiClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<TenantViewModel>> GetTenantsByIds(IEnumerable<Guid> ids)
    {
        var request = new GetTenantsByIdsRequest();

        request.Ids.AddRange(ids.Select(id => id.ToString()));

        var result = await _client.GetByIdsAsync(request);

        return result.Tenants.Select(tenant => new TenantViewModel(
            Guid.Parse(tenant.Id),
            tenant.Name,
            string.IsNullOrWhiteSpace(tenant.DeletedAt) ? null : DateTimeOffset.Parse(tenant.DeletedAt)));
    }
}