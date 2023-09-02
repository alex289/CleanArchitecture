using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Shared.Events.Tenant;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;

public sealed class DeleteTenantCommandHandler : CommandHandlerBase,
    IRequestHandler<DeleteTenantCommand>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;

    public DeleteTenantCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        ITenantRepository tenantRepository,
        IUserRepository userRepository,
        IUser user) : base(bus, unitOfWork, notifications)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _user = user;
    }

    public async Task Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
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
                    $"No permission to delete tenant {request.AggregateId}",
                    ErrorCodes.InsufficientPermissions));

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

        var tenantUsers = _userRepository
            .GetAll()
            .Where(x => x.TenantId == request.AggregateId);

        _userRepository.RemoveRange(tenantUsers);

        _tenantRepository.Remove(tenant);

        if (await CommitAsync())
        {
            await Bus.RaiseEventAsync(new TenantDeletedEvent(tenant.Id));
        }
    }
}