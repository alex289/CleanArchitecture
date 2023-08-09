using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
using NSubstitute;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.UpdateUser;

public sealed class UpdateUserCommandHandlerTests
{
    private readonly UpdateUserCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Update_User()
    {
        var user = _fixture.SetupUser();

        var command = new UpdateUserCommand(
            user.Id,
            "test@email.com",
            "Test",
            "Email",
            UserRole.User);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<UserUpdatedEvent>(x => x.UserId == command.UserId);
    }

    [Fact]
    public async Task Should_Not_Update_Non_Existing_User()
    {
        _fixture.SetupUser();

        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "test@email.com",
            "Test",
            "Email",
            UserRole.User);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no User with Id {command.UserId}");
    }

    [Fact]
    public async Task Should_Not_Update_With_Existing_User_Email()
    {
        var user = _fixture.SetupUser();

        var command = new UpdateUserCommand(
            user.Id,
            "test@email.com",
            "Test",
            "Email",
            UserRole.User);

        _fixture.UserRepository
            .GetByEmailAsync(command.Email)
            .Returns(new Entities.User(
                Guid.NewGuid(),
                command.Email,
                "Some",
                "User",
                "234fs@#*@#",
                UserRole.User));

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                DomainErrorCodes.UserAlreadyExists,
                $"There is already a User with Email {command.Email}");
    }
}