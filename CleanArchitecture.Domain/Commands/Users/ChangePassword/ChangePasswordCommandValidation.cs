using CleanArchitecture.Domain.Extensions.Validation;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.ChangePassword;

public sealed class ChangePasswordCommandValidation : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidation()
    {
        AddRuleForPassword();
        AddRuleForNewPassword();
    }

    private void AddRuleForPassword()
    {
        RuleFor(cmd => cmd.Password)
            .Password();
    }

    private void AddRuleForNewPassword()
    {
        RuleFor(cmd => cmd.NewPassword)
            .Password();
    }
}