using CleanArchitecture.Domain.Constants;
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
            .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
            .WithMessage("Email is not a valid email address")
            .MaximumLength(MaxLengths.User.Email)
            .WithErrorCode(DomainErrorCodes.User.EmailExceedsMaxLength)
            .WithMessage($"Email may not be longer than {MaxLengths.User.Email} characters");
    }

    private void AddRuleForPassword()
    {
        RuleFor(cmd => cmd.Password)
            .Password();
    }
}