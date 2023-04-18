using System;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommand : CommandBase
{
    private readonly CreateUserCommandValidation _validation = new();

    public CreateUserCommand(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string password) : base(userId)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Password { get; }

    public override bool IsValid()
    {
        ValidationResult = _validation.Validate(this);
        return ValidationResult.IsValid;
    }
}