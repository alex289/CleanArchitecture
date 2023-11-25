using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Shared.Events.Tenant;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Tenants.CreateTenant;

public sealed class CreateTenantCommandHandler : CommandHandlerBase,
    IRequestHandler<CreateTenantCommand>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUser _user;

    public CreateTenantCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        ITenantRepository tenantRepository,
        IUser user) : base(bus, unitOfWork, notifications)
    {
        _tenantRepository = tenantRepository;
        _user = user;
    }

    public async Task Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        if (_user.GetUserRole() != UserRole.Admin)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"No permission to create tenant {request.AggregateId}",
                    ErrorCodes.InsufficientPermissions));

            return;
        }

        if (await _tenantRepository.ExistsAsync(request.AggregateId))
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is already a tenant with Id {request.AggregateId}",
                    DomainErrorCodes.Tenant.AlreadyExists));

            return;
        }

        var tenant = new Tenant(
            request.AggregateId,
            request.Name);

        _tenantRepository.Add(tenant);

        if (await CommitAsync())
        {
            await Bus.RaiseEventAsync(new TenantCreatedEvent(
                tenant.Id,
                tenant.Name));
        }
    }
}