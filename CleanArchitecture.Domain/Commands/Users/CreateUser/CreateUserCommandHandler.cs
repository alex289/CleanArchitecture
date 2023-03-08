using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommandHandler : CommandHandlerBase,
    IRequestHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var existingUser = await _userRepository.GetByIdAsync(request.UserId);

        if (existingUser != null)
        {
            await _bus.RaiseEventAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is already a User with Id {request.UserId}",
                    DomainErrorCodes.UserAlreadyExists));
            return;
        }

        var user = new User(
            request.UserId, 
            request.Email,
            request.Surname,
            request.GivenName);
        
        _userRepository.Add(user);
        
        if (await CommitAsync())
        {
            await _bus.RaiseEventAsync(new UserCreatedEvent(user.Id));
        }
    }
}