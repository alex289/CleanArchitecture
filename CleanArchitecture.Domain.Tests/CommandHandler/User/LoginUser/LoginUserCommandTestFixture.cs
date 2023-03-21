using CleanArchitecture.Domain.Commands.Users.LoginUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.LoginUser;

public sealed class LoginUserCommandTestFixture : CommandHandlerFixtureBase
{
    public LoginUserCommandHandler CommandHandler { get; set; }
    public Mock<IUserRepository> UserRepository { get; set; }
    public IOptions<TokenSettings> TokenSettings { get; set; }

    public LoginUserCommandTestFixture()
    {
        UserRepository = new Mock<IUserRepository>();

        TokenSettings = Options.Create(new TokenSettings
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            Secret = "asjdlkasjd87439284)@#(*"
        });

        CommandHandler = new(
            Bus.Object,
            UnitOfWork.Object,
            NotificationHandler.Object,
            UserRepository.Object,
            TokenSettings);
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
            .Setup(x => x.GetByEmailAsync(It.Is<string>(y => y == user.Email)))
            .ReturnsAsync(user);

        return user;
    }
}
