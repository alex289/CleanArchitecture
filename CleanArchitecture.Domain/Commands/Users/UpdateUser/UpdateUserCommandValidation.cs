using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.UpdateUser;

public sealed class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        AddRuleForId();
        AddRuleForTenantId();
        AddRuleForEmail();
        AddRuleForFirstName();
        AddRuleForLastName();
        AddRuleForRole();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyId)
            .WithMessage("User id may not be empty");
    }

    private void AddRuleForTenantId()
    {
        RuleFor(cmd => cmd.TenantId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.EmptyId)
            .WithMessage("Tenant id may not be empty");
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

    private void AddRuleForFirstName()
    {
        RuleFor(cmd => cmd.FirstName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyFirstName)
            .WithMessage("FirstName may not be empty")
            .MaximumLength(MaxLengths.User.FirstName)
            .WithErrorCode(DomainErrorCodes.User.FirstNameExceedsMaxLength)
            .WithMessage($"FirstName may not be longer than {MaxLengths.User.FirstName} characters");
    }

    private void AddRuleForLastName()
    {
        RuleFor(cmd => cmd.LastName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyLastName)
            .WithMessage("LastName may not be empty")
            .MaximumLength(MaxLengths.User.LastName)
            .WithErrorCode(DomainErrorCodes.User.LastNameExceedsMaxLength)
            .WithMessage($"LastName may not be longer than {MaxLengths.User.LastName} characters");
    }

    private void AddRuleForRole()
    {
        RuleFor(cmd => cmd.Role)
            .IsInEnum()
            .WithErrorCode(DomainErrorCodes.User.InvalidRole)
            .WithMessage("Role is not a valid role");
    }
}