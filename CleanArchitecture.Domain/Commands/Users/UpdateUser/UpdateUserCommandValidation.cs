using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.UpdateUser;

public sealed class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        AddRuleForId();
        AddRuleForEmail();
        AddRuleForFirstName();
        AddRuleForLastName();
        AddRuleForRole();
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
            .MaximumLength(MaxLengths.User.Email)
            .WithErrorCode(DomainErrorCodes.UserEmailExceedsMaxLength)
            .WithMessage("Email may not be longer than 320 characters");
    }

    private void AddRuleForFirstName()
    {
        RuleFor(cmd => cmd.FirstName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyFirstName)
            .WithMessage("FirstName may not be empty")
            .MaximumLength(MaxLengths.User.FirstName)
            .WithErrorCode(DomainErrorCodes.UserFirstNameExceedsMaxLength)
            .WithMessage("FirstName may not be longer than 100 characters");
    }

    private void AddRuleForLastName()
    {
        RuleFor(cmd => cmd.LastName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyLastName)
            .WithMessage("LastName may not be empty")
            .MaximumLength(MaxLengths.User.LastName)
            .WithErrorCode(DomainErrorCodes.UserLastNameExceedsMaxLength)
            .WithMessage("LastName may not be longer than 100 characters");
    }

    private void AddRuleForRole()
    {
        RuleFor(cmd => cmd.Role)
            .IsInEnum()
            .WithErrorCode(DomainErrorCodes.UserInvalidRole)
            .WithMessage("Role is not a valid role");
    }
}