using System.Threading;
using System.Threading.Tasks;
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
    private readonly IUserRepository _userRepository;
    
    public UpdateUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
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
            await _bus.RaiseEventAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no User with Id {request.UserId}",
                    ErrorCodes.ObjectNotFound));
            return;
        }
        
        user.SetEmail(request.Email);
        user.SetSurname(request.Surname);
        user.SetGivenName(request.GivenName);
        
        _userRepository.Update(user);
        
        if (await CommitAsync())
        {
            await _bus.RaiseEventAsync(new UserUpdatedEvent(user.Id));
        }
    }
}