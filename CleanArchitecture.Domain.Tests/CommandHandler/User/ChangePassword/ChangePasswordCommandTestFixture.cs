using System;
using CleanArchitecture.Domain.Commands.Users.ChangePassword;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using NSubstitute;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.ChangePassword;

public sealed class ChangePasswordCommandTestFixture : CommandHandlerFixtureBase
{
    public ChangePasswordCommandHandler CommandHandler { get; }
    private IUserRepository UserRepository { get; }

    public ChangePasswordCommandTestFixture()
    {
        UserRepository = Substitute.For<IUserRepository>();

        CommandHandler = new ChangePasswordCommandHandler(
            Bus,
            UnitOfWork,
            NotificationHandler,
            UserRepository,
            User);
    }

    public Entities.User SetupUser()
    {
        var user = new Entities.User(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "max@mustermann.com",
            "Max",
            "Mustermann",
            BC.HashPassword("z8]tnayvd5FNLU9:]AQm"),
            UserRole.User);

        User.GetUserId().Returns(user.Id);

        UserRepository
            .GetByIdAsync(Arg.Is<Guid>(y => y == user.Id))
            .Returns(user);

        return user;
    }

    public Guid SetupMissingUser()
    {
        var id = Guid.NewGuid();
        User.GetUserId().Returns(id);
        return id;
    }
}