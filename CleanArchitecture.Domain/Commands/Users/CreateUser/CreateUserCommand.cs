using System;

namespace CleanArchitecture.Domain.Commands.Users.CreateUser;

public sealed class CreateUserCommand : CommandBase
{
    private readonly CreateUserCommandValidation _validation = new();

    public CreateUserCommand(
        Guid userId,
        string email,
        string surname,
        string givenName,
        string password) : base(userId)
    {
        UserId = userId;
        Email = email;
        Surname = surname;
        GivenName = givenName;
        Password = password;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string Surname { get; }
    public string GivenName { get; }
    public string Password { get; }

    public override bool IsValid()
    {
        ValidationResult = _validation.Validate(this);
        return ValidationResult.IsValid;
    }
}