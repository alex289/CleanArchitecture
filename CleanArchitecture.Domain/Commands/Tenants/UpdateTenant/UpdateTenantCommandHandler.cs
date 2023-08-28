using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.Tenant;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;

public sealed class UpdateTenantCommandHandler : CommandHandlerBase,
    IRequestHandler<UpdateTenantCommand>
{
    private readonly ITenantRepository _tenantRepository;
    
    public UpdateTenantCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        ITenantRepository tenantRepository) : base(bus, unitOfWork, notifications)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var tenant = await _tenantRepository.GetByIdAsync(request.AggregateId);

        if (tenant is null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no tenant with Id {request.AggregateId}",
                    ErrorCodes.ObjectNotFound));

            return;
        }
        
        tenant.SetName(request.Name);
        
        if (await CommitAsync())
        {
            await Bus.RaiseEventAsync(new TenantUpdatedEvent(
                tenant.Id,
                tenant.Name));
        }
    }
}