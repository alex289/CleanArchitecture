using System;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommand : CommandBase
{
    private static readonly CreateUserCommandValidation s_validation = new();

    public Guid UserId { get; }
    public Guid TenantId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Password { get; }

    public CreateUserCommand(
        Guid userId,
        Guid tenantId,
        string email,
        string firstName,
        string lastName,
        string password) : base(userId)
    {
        UserId = userId;
        TenantId = tenantId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}