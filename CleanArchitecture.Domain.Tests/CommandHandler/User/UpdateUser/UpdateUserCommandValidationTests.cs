using System;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.UpdateUser;

public sealed class UpdateUserCommandValidationTests :
    ValidationTestBase<UpdateUserCommand, UpdateUserCommandValidation>
{
    public UpdateUserCommandValidationTests() : base(new UpdateUserCommandValidation())
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

    [Fact]
    public void Should_Be_Invalid_For_Empty_Email()
    {
        var command = CreateTestCommand(email: string.Empty);

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.InvalidEmail,
            "Email is not a valid email address");
    }

    [Fact]
    public void Should_Be_Invalid_For_Invalid_Email()
    {
        var command = CreateTestCommand(email: "not a email");

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.InvalidEmail,
            "Email is not a valid email address");
    }

    [Fact]
    public void Should_Be_Invalid_For_Email_Exceeds_Max_Length()
    {
        var command = CreateTestCommand(email: new string('a', MaxLengths.User.Email) + "@test.com");

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.EmailExceedsMaxLength,
            $"Email may not be longer than {MaxLengths.User.Email} characters");
    }

    [Fact]
    public void Should_Be_Invalid_For_Empty_First_Name()
    {
        var command = CreateTestCommand(firstName: "");

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.EmptyFirstName,
            "FirstName may not be empty");
    }

    [Fact]
    public void Should_Be_Invalid_For_First_Name_Exceeds_Max_Length()
    {
        var command = CreateTestCommand(firstName: new string('a', MaxLengths.User.FirstName + 1));

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.FirstNameExceedsMaxLength,
            $"FirstName may not be longer than {MaxLengths.User.FirstName} characters");
    }

    [Fact]
    public void Should_Be_Invalid_For_Empty_Last_Name()
    {
        var command = CreateTestCommand(lastName: "");

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.EmptyLastName,
            "LastName may not be empty");
    }

    [Fact]
    public void Should_Be_Invalid_For_Last_Name_Exceeds_Max_Length()
    {
        var command = CreateTestCommand(lastName: new string('a', MaxLengths.User.LastName + 1));

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.User.LastNameExceedsMaxLength,
            $"LastName may not be longer than {MaxLengths.User.LastName} characters");
    }

    [Fact]
    public void Should_Be_Invalid_For_Empty_Tenant_Id()
    {
        var command = CreateTestCommand(tenantId: Guid.Empty);

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.Tenant.EmptyId,
            "Tenant id may not be empty");
    }

    private static UpdateUserCommand CreateTestCommand(
        Guid? userId = null,
        Guid? tenantId = null,
        string? email = null,
        string? firstName = null,
        string? lastName = null,
        UserRole? role = null)
    {
        return new UpdateUserCommand(
            userId ?? Guid.NewGuid(),
            email ?? "test@email.com",
            firstName ?? "test",
            lastName ?? "email",
            role ?? UserRole.User,
            tenantId ?? Guid.NewGuid());
    }
}