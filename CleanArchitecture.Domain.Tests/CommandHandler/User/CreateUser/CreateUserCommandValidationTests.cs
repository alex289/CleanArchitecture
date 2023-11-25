using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Commands.Users.CreateUser;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Errors;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.CreateUser;

public sealed class CreateUserCommandValidationTests :
    ValidationTestBase<CreateUserCommand, CreateUserCommandValidation>
{
    public CreateUserCommandValidationTests() : base(new CreateUserCommandValidation())
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
    public void Should_Be_Invalid_For_Empty_Password()
    {
        var command = CreateTestCommand(password: "");

        var errors = new List<string>
        {
            DomainErrorCodes.User.EmptyPassword,
            DomainErrorCodes.User.SpecialCharPassword,
            DomainErrorCodes.User.NumberPassword,
            DomainErrorCodes.User.LowercaseLetterPassword,
            DomainErrorCodes.User.UppercaseLetterPassword,
            DomainErrorCodes.User.ShortPassword
        };

        ShouldHaveExpectedErrors(command, errors.ToArray());
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Special_Character()
    {
        var command = CreateTestCommand(password: "z8tnayvd5FNLU9AQm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.SpecialCharPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Number()
    {
        var command = CreateTestCommand(password: "z]tnayvdFNLU:]AQm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.NumberPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Lowercase_Character()
    {
        var command = CreateTestCommand(password: "Z8]TNAYVDFNLU:]AQM");

        ShouldHaveSingleError(command, DomainErrorCodes.User.LowercaseLetterPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Uppercase_Character()
    {
        var command = CreateTestCommand(password: "z8]tnayvd5fnlu9:]aqm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UppercaseLetterPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Short()
    {
        var command = CreateTestCommand(password: "zA6{");

        ShouldHaveSingleError(command, DomainErrorCodes.User.ShortPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Long()
    {
        var command = CreateTestCommand(password: string.Concat(Enumerable.Repeat("zA6{", 12), 12));

        ShouldHaveSingleError(command, DomainErrorCodes.User.LongPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Empty_Tenant_Id()
    {
        var command = CreateTestCommand(tenantId: Guid.Empty);

        ShouldHaveSingleError(command, DomainErrorCodes.Tenant.EmptyId);
    }

    private static CreateUserCommand CreateTestCommand(
        Guid? userId = null,
        Guid? tenantId = null,
        string? email = null,
        string? firstName = null,
        string? lastName = null,
        string? password = null)
    {
        return new CreateUserCommand(
            userId ?? Guid.NewGuid(),
            tenantId ?? Guid.NewGuid(),
            email ?? "test@email.com",
            firstName ?? "test",
            lastName ?? "email",
            password ?? "Po=PF]PC6t.?8?ks)A6W");
    }
}