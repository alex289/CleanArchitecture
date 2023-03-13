using System;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Errors;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.DeleteUser;

public sealed class DeleteUserCommandValidationTests :
    ValidationTestBase<DeleteUserCommand, DeleteUserCommandValidation>
{
    public DeleteUserCommandValidationTests() : base(new DeleteUserCommandValidation())
    {
    }

    [Fact]
    public void Should_Be_Valid()
    {
        var command = CreateTestCommand();
        
        ShouldBeValid(command);
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Empty_User_Id()
    {
        var command = CreateTestCommand(userId: Guid.Empty);
        
        ShouldHaveSingleError(
            command, 
            DomainErrorCodes.UserEmptyId,
            "User id may not be empty");
    }
    
    private DeleteUserCommand CreateTestCommand(Guid? userId = null) => 
        new (userId ?? Guid.NewGuid());
}