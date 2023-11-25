using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Shared.Events.User;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : CommandHandlerBase,
    IRequestHandler<UpdateUserCommand>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository,
        IUser user,
        ITenantRepository tenantRepository) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
        _user = user;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no user with Id {request.UserId}",
                    ErrorCodes.ObjectNotFound));
            return;
        }

        if (_user.GetUserId() != request.UserId && _user.GetUserRole() != UserRole.Admin)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"No permission to update user {request.UserId}",
                    ErrorCodes.InsufficientPermissions));

            return;
        }

        if (request.Email != user.Email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is already a user with email {request.Email}",
                        DomainErrorCodes.User.AlreadyExists));
                return;
            }
        }

        if (_user.GetUserRole() == UserRole.Admin)
        {
            user.SetRole(request.Role);

            if (!await _tenantRepository.ExistsAsync(request.TenantId))
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is no tenant with Id {request.TenantId}",
                        ErrorCodes.ObjectNotFound));
                return;
            }

            user.SetTenant(request.TenantId);
        }

        user.SetEmail(request.Email);
        user.SetFirstName(request.FirstName);
        user.SetLastName(request.LastName);

        _userRepository.Update(user);

        if (await CommitAsync())
        {
            await Bus.RaiseEventAsync(new UserUpdatedEvent(user.Id, user.TenantId));
        }
    }
}