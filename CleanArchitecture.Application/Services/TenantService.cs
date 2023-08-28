using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries.Tenants.GetAll;
using CleanArchitecture.Application.Queries.Tenants.GetTenantById;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain.Commands.Tenants.CreateTenant;
using CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;
using CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;
using CleanArchitecture.Domain.Interfaces;

namespace CleanArchitecture.Application.Services;

public sealed class TenantService : ITenantService
{
    private readonly IMediatorHandler _bus;

    public TenantService(IMediatorHandler bus)
    {
        _bus = bus;
    }

    public async Task<Guid> CreateTenantAsync(CreateTenantViewModel tenant)
    {
        var tenantId = Guid.NewGuid();
        
        await _bus.SendCommandAsync(new CreateTenantCommand(
            tenantId,
            tenant.Name));
        
        return tenantId;
    }

    public async Task UpdateTenantAsync(UpdateTenantViewModel tenant)
    {
        await _bus.SendCommandAsync(new UpdateTenantCommand(
            tenant.Id,
            tenant.Name));
    }

    public async Task DeleteTenantAsync(Guid tenantId)
    {
        await _bus.SendCommandAsync(new DeleteTenantCommand(tenantId));
    }

    public async Task<TenantViewModel?> GetTenantByIdAsync(Guid tenantId, bool deleted)
    {
        return await _bus.QueryAsync(new GetTenantByIdQuery(tenantId, deleted));
    }

    public async Task<IEnumerable<TenantViewModel>> GetAllTenantsAsync()
    {
        return await _bus.QueryAsync(new GetAllTenantsQuery());
    }
}