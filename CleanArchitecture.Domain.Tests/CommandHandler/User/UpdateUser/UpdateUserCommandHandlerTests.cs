using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
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
            "Email");
        
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
            "Email");
        
        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no User with Id {command.UserId}");
    }
}