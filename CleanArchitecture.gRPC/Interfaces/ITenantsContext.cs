using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Tenants;

namespace CleanArchitecture.gRPC.Interfaces;

public interface ITenantsContext
{
    Task<IEnumerable<TenantViewModel>> GetTenantsByIds(IEnumerable<Guid> ids);
}