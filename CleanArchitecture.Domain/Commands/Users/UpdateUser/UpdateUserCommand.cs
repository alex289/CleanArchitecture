using System;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Commands.Users.UpdateUser;

public sealed class UpdateUserCommand : CommandBase
{
    private readonly UpdateUserCommandValidation _validation = new();

    public UpdateUserCommand(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        UserRole role) : base(userId)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public UserRole Role { get; }

    public override bool IsValid()
    {
        ValidationResult = _validation.Validate(this);
        return ValidationResult.IsValid;
    }
}