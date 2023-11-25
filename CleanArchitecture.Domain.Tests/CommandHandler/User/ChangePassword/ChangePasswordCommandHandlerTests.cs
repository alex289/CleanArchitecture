using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.ChangePassword;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Shared.Events.User;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.ChangePassword;

public sealed class ChangePasswordCommandHandlerTests
{
    private readonly ChangePasswordCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Change_Password()
    {
        var user = _fixture.SetupUser();

        var command = new ChangePasswordCommand("z8]tnayvd5FNLU9:]AQm", "z8]tnayvd5FNLU9:]AQw");

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<PasswordChangedEvent>(x => x.AggregateId == user.Id);
    }

    [Fact]
    public async Task Should_Not_Change_Password_No_User()
    {
        var userId = _fixture.SetupMissingUser();

        var command = new ChangePasswordCommand("z8]tnayvd5FNLU9:]AQm", "z8]tnayvd5FNLU9:]AQw");

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no user with Id {userId}");
    }

    [Fact]
    public async Task Should_Not_Change_Password_Incorrect_Password()
    {
        _fixture.SetupUser();

        var command = new ChangePasswordCommand("z8]tnayvd5FNLU9:]AQw", "z8]tnayvd5FNLU9:]AQx");

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                DomainErrorCodes.User.PasswordIncorrect,
                "The password is incorrect");
    }
}