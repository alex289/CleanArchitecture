﻿using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Commands.Users.ChangePassword;

public sealed class ChangePasswordCommandHandler : CommandHandlerBase,
    IRequestHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUser _user;

    public ChangePasswordCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository,
        IUser user) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
        _user = user;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(_user.GetUserId());

        if (user == null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no User with Id {_user.GetUserId()}",
                    ErrorCodes.ObjectNotFound));

            return;
        }

        if (!BC.Verify(request.Password, user.Password))
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    "The password is incorrect",
                    DomainErrorCodes.UserPasswordIncorrect));

            return;
        }

        string passwordHash = BC.HashPassword(request.NewPassword);
        user.SetPassword(passwordHash);

        _userRepository.Update(user);

        if (await CommitAsync())
        {
            await _bus.RaiseEventAsync(new PasswordChangedEvent(user.Id));
        }
    }
}