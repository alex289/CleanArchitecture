using System;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Commands.Users.UpdateUser;

public sealed class UpdateUserCommand : CommandBase
{
    private static readonly UpdateUserCommandValidation s_validation = new();

    public Guid UserId { get; }
    public Guid TenantId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public UserRole Role { get; }

    public UpdateUserCommand(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        UserRole role, Guid tenantId) : base(userId)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        TenantId = tenantId;
    }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}