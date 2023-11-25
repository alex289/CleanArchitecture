using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Users.DeleteUser;

public sealed class DeleteUserCommandValidation : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidation()
    {
        AddRuleForId();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.UserId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyId)
            .WithMessage("User id may not be empty");
    }
}