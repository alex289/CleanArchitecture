using System;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.DeleteUser;

public sealed class DeleteUserCommandTestFixture : CommandHandlerFixtureBase
{
    public DeleteUserCommandTestFixture()
    {
        UserRepository = Substitute.For<IUserRepository>();

        CommandHandler = new DeleteUserCommandHandler(
            Bus,
            UnitOfWork,
            NotificationHandler,
            UserRepository,
            User);
    }

    public DeleteUserCommandHandler CommandHandler { get; }
    private IUserRepository UserRepository { get; }

    public Entities.User SetupUser()
    {
        var user = new Entities.User(
            Guid.NewGuid(),
            "max@mustermann.com",
            "Max",
            "Mustermann",
            "Password",
            UserRole.User);

        UserRepository
            .GetByIdAsync(Arg.Is<Guid>(y => y == user.Id))
            .Returns(user);

        return user;
    }
}