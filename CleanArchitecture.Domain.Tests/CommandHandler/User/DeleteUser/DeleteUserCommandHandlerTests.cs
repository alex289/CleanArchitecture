using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Shared.Events.User;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.DeleteUser;

public sealed class DeleteUserCommandHandlerTests
{
    private readonly DeleteUserCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Delete_User()
    {
        var user = _fixture.SetupUser();

        var command = new DeleteUserCommand(user.Id);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<UserDeletedEvent>(x => x.AggregateId == user.Id);
    }

    [Fact]
    public async Task Should_Not_Delete_Non_Existing_User()
    {
        _fixture.SetupUser();

        var command = new DeleteUserCommand(Guid.NewGuid());

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserDeletedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no user with Id {command.UserId}");
    }

    [Fact]
    public async Task Should_Not_Delete_User_Insufficient_Permissions()
    {
        var user = _fixture.SetupUser();

        _fixture.SetupCurrentUser();

        var command = new DeleteUserCommand(user.Id);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserDeletedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.InsufficientPermissions,
                $"No permission to delete user {command.UserId}");
    }
}