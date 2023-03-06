using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Users.DeleteUser;

public sealed class DeleteUserCommandHandler : CommandHandlerBase,
    IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);
        
        if (user == null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no User with Id {request.UserId}",
                    ErrorCodes.ObjectNotFound));

            return;
        }

        _userRepository.Remove(user);

        if (!await CommitAsync())
        {
            await _bus.RaiseEventAsync(new UserDeletedEvent(request.UserId));
        }
    }
}