using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Proto.Tenants;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.gRPC;

public sealed class TenantsApiImplementation : TenantsApi.TenantsApiBase
{
    private readonly ITenantRepository _tenantRepository;

    public TenantsApiImplementation(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public override async Task<GetTenantsByIdsResult> GetByIds(
        GetTenantsByIdsRequest request,
        ServerCallContext context)
    {
        var idsAsGuids = new List<Guid>(request.Ids.Count);

        foreach (var id in request.Ids)
        {
            if (Guid.TryParse(id, out var parsed))
            {
                idsAsGuids.Add(parsed);
            }
        }

        var tenants = await _tenantRepository
            .GetAllNoTracking()
            .IgnoreQueryFilters()
            .Where(tenant => idsAsGuids.Contains(tenant.Id))
            .Select(tenant => new Tenant
            {
                Id = tenant.Id.ToString(),
                Name = tenant.Name,
                DeletedAt = tenant.DeletedAt == null ? "": tenant.DeletedAt.ToString()
            })
            .ToListAsync();

        var result = new GetTenantsByIdsResult();

        result.Tenants.AddRange(tenants);

        return result;
    }
}