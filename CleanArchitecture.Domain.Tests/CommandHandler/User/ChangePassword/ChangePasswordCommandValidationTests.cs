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
            DomainErrorCodes.User.UserEmptyPassword,
            DomainErrorCodes.User.UserSpecialCharPassword,
            DomainErrorCodes.User.UserNumberPassword,
            DomainErrorCodes.User.UserLowercaseLetterPassword,
            DomainErrorCodes.User.UserUppercaseLetterPassword,
            DomainErrorCodes.User.UserShortPassword
        };

        ShouldHaveExpectedErrors(command, errors.ToArray());
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Special_Character()
    {
        var command = CreateTestCommand("z8tnayvd5FNLU9AQm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UserSpecialCharPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Number()
    {
        var command = CreateTestCommand("z]tnayvdFNLU:]AQm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UserNumberPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Lowercase_Character()
    {
        var command = CreateTestCommand("Z8]TNAYVDFNLU:]AQM");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UserLowercaseLetterPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Missing_Uppercase_Character()
    {
        var command = CreateTestCommand("z8]tnayvd5fnlu9:]aqm");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UserUppercaseLetterPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Short()
    {
        var command = CreateTestCommand("zA6{");

        ShouldHaveSingleError(command, DomainErrorCodes.User.UserShortPassword);
    }

    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Long()
    {
        var command = CreateTestCommand(string.Concat(Enumerable.Repeat("zA6{", 12), 12));

        ShouldHaveSingleError(command, DomainErrorCodes.User.UserLongPassword);
    }

    private static ChangePasswordCommand CreateTestCommand(
        string? password = null, string? newPassword = null)
    {
        return new(
            password ?? "z8]tnayvd5FNLU9:]AQm",
            newPassword ?? "z8]tnayvd5FNLU9:]AQw");
    }
}