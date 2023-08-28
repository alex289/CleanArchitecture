using System;
using MediatR;

namespace CleanArchitecture.Domain.Commands.Users.LoginUser;

public sealed class LoginUserCommand : CommandBase,
    IRequest<string>
{
    private static readonly LoginUserCommandValidation s_validation = new();


    public LoginUserCommand(
        string email,
        string password) : base(Guid.NewGuid())
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }
    public string Password { get; set; }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}