using CleanArchitecture.Domain.Commands.Users.LoginUser;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.LoginUser;

public sealed class LoginUserCommandValidationTests :
    ValidationTestBase<LoginUserCommand, LoginUserCommandValidation>
{
    public LoginUserCommandValidationTests() : base(new LoginUserCommandValidation())
    {
    }
}
