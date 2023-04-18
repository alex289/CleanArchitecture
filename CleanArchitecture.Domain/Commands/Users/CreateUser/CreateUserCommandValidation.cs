using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Extensions.Validation;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommandValidation : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidation()
    {
        AddRuleForId();
        AddRuleForEmail();
        AddRuleForFirstName();
        AddRuleForLastName();
        AddRuleForPassword();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyId)
            .WithMessage("User id may not be empty");
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

    private void AddRuleForFirstName()
    {
        RuleFor(cmd => cmd.FirstName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyFirstName)
            .WithMessage("FirstName may not be empty")
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserFirstNameExceedsMaxLength)
            .WithMessage("FirstName may not be longer than 100 characters");
    }

    private void AddRuleForLastName()
    {
        RuleFor(cmd => cmd.LastName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyLastName)
            .WithMessage("LastName may not be empty")
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserLastNameExceedsMaxLength)
            .WithMessage("LastName may not be longer than 100 characters");
    }

    private void AddRuleForPassword()
    {
        RuleFor(cmd => cmd.Password)
            .Password();
    }
}