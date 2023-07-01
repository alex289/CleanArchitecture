using System;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Moq;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.UpdateUser;

public sealed class UpdateUserCommandTestFixture : CommandHandlerFixtureBase
{
    public UpdateUserCommandTestFixture()
    {
        UserRepository = new Mock<IUserRepository>();

        CommandHandler = new UpdateUserCommandHandler(
            Bus.Object,
            UnitOfWork.Object,
            NotificationHandler.Object,
            UserRepository.Object,
            User.Object);
    }

    public UpdateUserCommandHandler CommandHandler { get; }
    public Mock<IUserRepository> UserRepository { get; }

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
            .Setup(x => x.GetByIdAsync(It.Is<Guid>(y => y == user.Id)))
            .ReturnsAsync(user);

        return user;
    }
}