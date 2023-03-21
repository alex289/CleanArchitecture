using System;
using CleanArchitecture.Domain.Commands.Users.ChangePassword;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Moq;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.ChangePassword;

public sealed class ChangePasswordCommandTestFixture : CommandHandlerFixtureBase
{
    public ChangePasswordCommandHandler CommandHandler { get; set; }
    public Mock<IUserRepository> UserRepository { get; set; }

    public ChangePasswordCommandTestFixture()
    {
        UserRepository = new Mock<IUserRepository>();

        CommandHandler = new(
            Bus.Object,
            UnitOfWork.Object,
            NotificationHandler.Object,
            UserRepository.Object,
            User.Object);
    }

    public Entities.User SetupUser()
    {
        var user = new Entities.User(
            Guid.NewGuid(),
            "max@mustermann.com",
            "Max",
            "Mustermann",
            BC.HashPassword("z8]tnayvd5FNLU9:]AQm"),
            UserRole.User);

        User.Setup(x => x.GetUserId()).Returns(user.Id);

        UserRepository
            .Setup(x => x.GetByIdAsync(It.Is<Guid>(y => y == user.Id)))
            .ReturnsAsync(user);

        return user;
    }

    public Guid SetupMissingUser()
    {
        var id = Guid.NewGuid();
        User.Setup(x => x.GetUserId()).Returns(id);
        return id;
    }
}
