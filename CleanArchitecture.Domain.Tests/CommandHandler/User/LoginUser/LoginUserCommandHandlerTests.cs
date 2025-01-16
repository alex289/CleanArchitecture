using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.LoginUser;
using CleanArchitecture.Domain.Errors;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.LoginUser;

public sealed class LoginUserCommandHandlerTests
{
    private readonly LoginUserCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Login_User()
    {
        var user = _fixture.SetupUser();

        var command = new LoginUserCommand(user.Email, "z8]tnayvd5FNLU9:]AQm");

        var token = await _fixture.CommandHandler.Handle(command, default);

        _fixture.VerifyNoDomainNotification();

        token.ShouldNotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadToken(token) as JwtSecurityToken;

        var userIdClaim = decodedToken!.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.NameIdentifier));

        Guid.Parse(userIdClaim!.Value).ShouldBe(user.Id);

        var userEmailClaim = decodedToken.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Email));

        userEmailClaim!.Value.ShouldBe(user.Email);

        var userRoleClaim = decodedToken.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Role));

        userRoleClaim!.Value.ShouldBe(user.Role.ToString());
    }

    [Fact]
    public async Task Should_Not_Login_User_No_User()
    {
        var command = new LoginUserCommand("test@email.com", "z8]tnayvd5FNLU9:]AQm");

        var token = await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no user with email {command.Email}");

        token.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Not_Login_User_Incorrect_Password()
    {
        var user = _fixture.SetupUser();

        var command = new LoginUserCommand(user.Email, "z8]tnayvd5FNLU9:]AQw");

        var token = await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                DomainErrorCodes.User.PasswordIncorrect,
                "The password is incorrect");

        token.ShouldBeEmpty();
    }
}