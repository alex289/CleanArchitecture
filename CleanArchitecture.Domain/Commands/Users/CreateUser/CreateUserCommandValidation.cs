using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Extensions.Validation;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommandValidation : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidation()
    {
        AddRuleForId();
        AddRuleForTenantId();
        AddRuleForEmail();
        AddRuleForFirstName();
        AddRuleForLastName();
        AddRuleForPassword();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.UserEmptyId)
            .WithMessage("User id may not be empty");
    }

    private void AddRuleForTenantId()
    {
        RuleFor(cmd => cmd.TenantId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.TenantEmptyId)
            .WithMessage("Tenant id may not be empty");
    }

    private void AddRuleForEmail()
    {
        RuleFor(cmd => cmd.Email)
            .EmailAddress()
            .WithErrorCode(DomainErrorCodes.User.UserInvalidEmail)
            .WithMessage("Email is not a valid email address")
            .MaximumLength(MaxLengths.User.Email)
            .WithErrorCode(DomainErrorCodes.User.UserEmailExceedsMaxLength)
            .WithMessage($"Email may not be longer than {MaxLengths.User.Email} characters");
    }

    private void AddRuleForFirstName()
    {
        RuleFor(cmd => cmd.FirstName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.UserEmptyFirstName)
            .WithMessage("FirstName may not be empty")
            .MaximumLength(MaxLengths.User.FirstName)
            .WithErrorCode(DomainErrorCodes.User.UserFirstNameExceedsMaxLength)
            .WithMessage($"FirstName may not be longer than {MaxLengths.User.FirstName} characters");
    }

    private void AddRuleForLastName()
    {
        RuleFor(cmd => cmd.LastName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.UserEmptyLastName)
            .WithMessage("LastName may not be empty")
            .MaximumLength(MaxLengths.User.LastName)
            .WithErrorCode(DomainErrorCodes.User.UserLastNameExceedsMaxLength)
            .WithMessage($"LastName may not be longer than {MaxLengths.User.LastName} characters");
    }

    private void AddRuleForPassword()
    {
        RuleFor(cmd => cmd.Password)
            .Password();
    }
}