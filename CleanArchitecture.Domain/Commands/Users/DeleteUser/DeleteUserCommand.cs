using System;

namespace CleanArchitecture.Domain.Commands.Users.DeleteUser;

public sealed class DeleteUserCommand : CommandBase
{
    private readonly DeleteUserCommandValidation _validation = new();

    public DeleteUserCommand(Guid userId) : base(userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }

    public override bool IsValid()
    {
        ValidationResult = _validation.Validate(this);
        return ValidationResult.IsValid;
    }
}