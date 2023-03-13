using System;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.User;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.DeleteUser;

public sealed class DeleteUserCommandHandlerTests
{
    private readonly DeleteUserCommandTestFixture _fixture = new();
    
    [Fact]
    public void Should_Delete_User()
    {
        var user = _fixture.SetupUser();
        
        var command = new DeleteUserCommand(user.Id);
        
        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<UserDeletedEvent>(x => x.UserId == user.Id);
    }
    
    [Fact]
    public void Should_Not_Delete_Non_Existing_User()
    {
        _fixture.SetupUser();
        
        var command = new DeleteUserCommand(Guid.NewGuid());
        
        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserDeletedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no User with Id {command.UserId}");
    }
}