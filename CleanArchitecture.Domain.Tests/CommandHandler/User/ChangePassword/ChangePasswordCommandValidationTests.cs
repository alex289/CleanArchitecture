using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Commands.Users.ChangePassword;
using CleanArchitecture.Domain.Errors;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.ChangePassword;

public sealed class ChangePasswordCommandValidationTests :
    ValidationTestBase<ChangePasswordCommand, ChangePasswordCommandValidation>
{
    public ChangePasswordCommandValidationTests() : base(new ChangePasswordCommandValidation())
    {
    }

    [Fact]
    public void Should_Be_Valid()
    {
        var command = CreateTestCommand();

        ShouldBeValid(command);
    }

    [Fact]
    public void Should_Be_Invalid_For_Empty_Password()
    {
        var command = CreateTestCommand("");

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
        var command = CreateTestCommand("z8tnayvd5FNLU9AQm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.SpecialCharPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Number()
    {
        var command = CreateTestCommand("z]tnayvdFNLU:]AQm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.NumberPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Lowercase_Character()
    {
        var command = CreateTestCommand("Z8]TNAYVDFNLU:]AQM");

        ShouldHaveSingleError(command, DomainErrorCodes.User.LowercaseLetterPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Uppercase_Character()
    {
        var command = CreateTestCommand("z8]tnayvd5fnlu9:]aqm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UppercaseLetterPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Short()
    {
        var command = CreateTestCommand("zA6{");

        ShouldHaveSingleError(command, DomainErrorCodes.User.ShortPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Long()
    {
        var command = CreateTestCommand(string.Concat(Enumerable.Repeat("zA6{", 12), 12));

        ShouldHaveSingleError(command, DomainErrorCodes.User.LongPassword);
    }

    private static ChangePasswordCommand CreateTestCommand(
        string? password = null, string? newPassword = null)
    {
        return new ChangePasswordCommand(
            password ?? "z8]tnayvd5FNLU9:]AQm",
            newPassword ?? "z8]tnayvd5FNLU9:]AQw");
    }
}