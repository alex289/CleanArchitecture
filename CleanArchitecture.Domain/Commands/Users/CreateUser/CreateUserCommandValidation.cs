using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommandValidation : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidation()
    {
        AddRuleForId();
        AddRuleForEmail();
        AddRuleForSurname();
        AddRuleForGivenName();
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
    
    private void AddRuleForSurname()
    {
        RuleFor(cmd => cmd.Surname)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptySurname)
            .WithMessage("Surname may not be empty")
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserSurnameExceedsMaxLength)
            .WithMessage("Surname may not be longer than 100 characters");
    }
    
    private void AddRuleForGivenName()
    {
        RuleFor(cmd => cmd.GivenName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyGivenName)
            .WithMessage("Given name may not be empty")
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserGivenNameExceedsMaxLength)
            .WithMessage("Given name may not be longer than 100 characters");
    }
}