using System;

namespace CleanArchitecture.Domain.Commands.Users.DeleteUser;

public sealed class DeleteUserCommand : CommandBase
{
    private static readonly DeleteUserCommandValidation s_validation = new();

    public DeleteUserCommand(Guid userId) : base(userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}