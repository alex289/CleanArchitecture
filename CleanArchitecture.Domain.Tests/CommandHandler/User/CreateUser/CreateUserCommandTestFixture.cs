using System;
using CleanArchitecture.Domain.Commands.Users.CreateUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Moq;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.CreateUser;

public sealed class CreateUserCommandTestFixture : CommandHandlerFixtureBase
{
    public CreateUserCommandTestFixture()
    {
        UserRepository = new Mock<IUserRepository>();

        CommandHandler = new CreateUserCommandHandler(
            Bus.Object,
            UnitOfWork.Object,
            NotificationHandler.Object,
            UserRepository.Object);
    }

    public CreateUserCommandHandler CommandHandler { get; }
    private Mock<IUserRepository> UserRepository { get; }

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