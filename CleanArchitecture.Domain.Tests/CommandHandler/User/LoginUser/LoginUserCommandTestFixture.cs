using System;
using CleanArchitecture.Domain.Commands.Users.LoginUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Settings;
using Microsoft.Extensions.Options;
using NSubstitute;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.LoginUser;

public sealed class LoginUserCommandTestFixture : CommandHandlerFixtureBase
{
    public LoginUserCommandHandler CommandHandler { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IOptions<TokenSettings> TokenSettings { get; set; }

    public LoginUserCommandTestFixture()
    {
        UserRepository = Substitute.For<IUserRepository>();

        TokenSettings = Options.Create(new TokenSettings
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            Secret = "asjdlkasjd87439284)@#(*asjdlkasjd87439284)@#(*"
        });

        CommandHandler = new LoginUserCommandHandler(
            Bus,
            UnitOfWork,
            NotificationHandler,
            UserRepository,
            TokenSettings);
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
            .GetByEmailAsync(Arg.Is<string>(y => y == user.Email))
            .Returns(user);

        return user;
    }
}