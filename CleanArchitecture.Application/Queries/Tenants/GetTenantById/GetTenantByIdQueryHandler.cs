using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tenants.GetTenantById;

public sealed class GetTenantByIdQueryHandler :
    IRequestHandler<GetTenantByIdQuery, TenantViewModel?>
{
    private readonly IMediatorHandler _bus;
    private readonly ITenantRepository _tenantRepository;

    public GetTenantByIdQueryHandler(ITenantRepository tenantRepository, IMediatorHandler bus)
    {
        _tenantRepository = tenantRepository;
        _bus = bus;
    }

    public async Task<TenantViewModel?> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);

        if (tenant is null)
        {
            await _bus.RaiseEventAsync(
                new DomainNotification(
                    nameof(GetTenantByIdQuery),
                    $"Tenant with id {request.TenantId} could not be found",
                    ErrorCodes.ObjectNotFound));
            return null;
        }

        return TenantViewModel.FromTenant(tenant);
    }
}