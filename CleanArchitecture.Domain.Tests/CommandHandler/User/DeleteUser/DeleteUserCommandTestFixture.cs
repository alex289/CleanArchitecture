using System;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Moq;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.DeleteUser;

public sealed class DeleteUserCommandTestFixture : CommandHandlerFixtureBase
{
    public DeleteUserCommandHandler CommandHandler { get; }
    private Mock<IUserRepository> UserRepository { get; }
    
    public DeleteUserCommandTestFixture()
    {
        UserRepository = new Mock<IUserRepository>();
        
        CommandHandler = new (
            Bus.Object,
            UnitOfWork.Object,
            NotificationHandler.Object,
            UserRepository.Object);
    }

    public Entities.User SetupUser()
    {
        var user = new Entities.User(
            Guid.NewGuid(),
            "max@mustermann.com",
            "Max",
            "Mustermann");

        UserRepository
            .Setup(x => x.GetByIdAsync(It.Is<Guid>(y => y == user.Id)))
            .ReturnsAsync(user);

        return user;
    }
}