using CleanArchitecture.Domain.Commands.Users.ChangePassword;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.ChangePassword;

public sealed class ChangePasswordCommandValidationTests :
    ValidationTestBase<ChangePasswordCommand, ChangePasswordCommandValidation>
{
    public ChangePasswordCommandValidationTests() : base(new ChangePasswordCommandValidation())
    {
    }
}
