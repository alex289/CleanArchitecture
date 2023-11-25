using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Shared.Events.User;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommandHandler : CommandHandlerBase,
    IRequestHandler<CreateUserCommand>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUser _user;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        IUser user) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _user = user;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var currentUser = await _userRepository.GetByIdAsync(_user.GetUserId());

        if (currentUser is null || currentUser.Role != UserRole.Admin)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    "You are not allowed to create users",
                    ErrorCodes.InsufficientPermissions));
            return;
        }

        var existingUser = await _userRepository.GetByIdAsync(request.UserId);

        if (existingUser is not null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is already a user with Id {request.UserId}",
                    DomainErrorCodes.User.AlreadyExists));
            return;
        }

        existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is already a user with email {request.Email}",
                    DomainErrorCodes.User.AlreadyExists));
            return;
        }

        if (!await _tenantRepository.ExistsAsync(request.TenantId))
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no tenant with Id {request.TenantId}",
                    ErrorCodes.ObjectNotFound));
            return;
        }

        var passwordHash = BC.HashPassword(request.Password);

        var user = new User(
            request.UserId,
            request.TenantId,
            request.Email,
            request.FirstName,
            request.LastName,
            passwordHash,
            UserRole.User);

        _userRepository.Add(user);

        if (await CommitAsync())
        {
            await Bus.RaiseEventAsync(new UserCreatedEvent(user.Id, user.TenantId));
        }
    }
}