using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Tenants;

namespace CleanArchitecture.Application.Interfaces;

public interface ITenantService
{
    public Task<Guid> CreateTenantAsync(CreateTenantViewModel tenant);
    public Task UpdateTenantAsync(UpdateTenantViewModel tenant);
    public Task DeleteTenantAsync(Guid tenantId);
    public Task<TenantViewModel?> GetTenantByIdAsync(Guid tenantId, bool deleted);
    public Task<IEnumerable<TenantViewModel>> GetAllTenantsAsync();
}