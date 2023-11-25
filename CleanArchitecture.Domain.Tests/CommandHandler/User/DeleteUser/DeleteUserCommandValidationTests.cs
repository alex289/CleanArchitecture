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
        var command = CreateTestCommand(Guid.Empty);

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.EmptyId,
            "User id may not be empty");
    }

    private static DeleteUserCommand CreateTestCommand(Guid? userId = null)
    {
        return new DeleteUserCommand(userId ?? Guid.NewGuid());
    }
}