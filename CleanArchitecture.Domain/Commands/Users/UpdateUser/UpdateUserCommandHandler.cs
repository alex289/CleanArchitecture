using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : CommandHandlerBase,
    IRequestHandler<UpdateUserCommand>
{
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository,
        IUser user) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
        _user = user;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            await Bus.RaiseEventAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no User with Id {request.UserId}",
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

            if (existingUser != null)
            {
                await Bus.RaiseEventAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is already a User with Email {request.Email}",
                        DomainErrorCodes.UserAlreadyExists));
                return;
            }
        }

        if (_user.GetUserRole() == UserRole.Admin)
        {
            user.SetRole(request.Role);
        }

        user.SetEmail(request.Email);
        user.SetFirstName(request.FirstName);
        user.SetLastName(request.LastName);

        _userRepository.Update(user);

        if (await CommitAsync())
        {
            await Bus.RaiseEventAsync(new UserUpdatedEvent(user.Id));
        }
    }
}