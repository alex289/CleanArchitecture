using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Extensions.Validation;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.LoginUser;

public sealed class LoginUserCommandValidation : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidation()
    {
        AddRuleForEmail();
        AddRuleForPassword();
    }

    private void AddRuleForEmail()
    {
        RuleFor(cmd => cmd.Email)
            .EmailAddress()
            .WithErrorCode(DomainErrorCodes.UserInvalidEmail)
            .WithMessage("Email is not a valid email address")
            .MaximumLength(320)
            .WithErrorCode(DomainErrorCodes.UserEmailExceedsMaxLength)
            .WithMessage("Email may not be longer than 320 characters");
    }

    private void AddRuleForPassword()
    {
        RuleFor(cmd => cmd.Password)
            .Password();
    }
}
