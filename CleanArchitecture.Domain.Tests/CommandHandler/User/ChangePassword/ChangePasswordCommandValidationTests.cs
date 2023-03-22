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
            DomainErrorCodes.UserEmptyPassword,
            DomainErrorCodes.UserSpecialCharPassword,
            DomainErrorCodes.UserNumberPassword,
            DomainErrorCodes.UserLowercaseLetterPassword,
            DomainErrorCodes.UserUppercaseLetterPassword,
            DomainErrorCodes.UserShortPassword
        };
        
        ShouldHaveExpectedErrors(command, errors.ToArray());
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Missing_Special_Character()
    {
        var command = CreateTestCommand("z8tnayvd5FNLU9AQm");
        
        ShouldHaveSingleError(command, DomainErrorCodes.UserSpecialCharPassword);
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Missing_Number()
    {
        var command = CreateTestCommand("z]tnayvdFNLU:]AQm");
        
        ShouldHaveSingleError(command, DomainErrorCodes.UserNumberPassword);
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Missing_Lowercase_Character()
    {
        var command = CreateTestCommand("Z8]TNAYVDFNLU:]AQM");
        
        ShouldHaveSingleError(command, DomainErrorCodes.UserLowercaseLetterPassword);
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Missing_Uppercase_Character()
    {
        var command = CreateTestCommand("z8]tnayvd5fnlu9:]aqm");
        
        ShouldHaveSingleError(command, DomainErrorCodes.UserUppercaseLetterPassword);
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Short()
    {
        var command = CreateTestCommand("zA6{");
        
        ShouldHaveSingleError(command, DomainErrorCodes.UserShortPassword);
    }
    
    [Fact]
    public void Should_Be_Invalid_For_Password_Too_Long()
    {
        var command = CreateTestCommand(string.Concat(Enumerable.Repeat("zA6{", 12), 12));
        
        ShouldHaveSingleError(command, DomainErrorCodes.UserLongPassword);
    }

    private ChangePasswordCommand CreateTestCommand(
        string? password = null, string? newPassword = null) => new(
        password ?? "z8]tnayvd5FNLU9:]AQm", 
            newPassword ?? "z8]tnayvd5FNLU9:]AQw");
}
