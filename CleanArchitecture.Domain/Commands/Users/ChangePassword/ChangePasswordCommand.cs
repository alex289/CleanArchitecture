using System;

namespace CleanArchitecture.Domain.Commands.Users.ChangePassword;

public sealed class ChangePasswordCommand : CommandBase
{
    private readonly ChangePasswordCommandValidation _validation = new();

    public ChangePasswordCommand(string password, string newPassword) : base(Guid.NewGuid())
    {
        Password = password;
        NewPassword = newPassword;
    }

    public string Password { get; }
    public string NewPassword { get; }

    public override bool IsValid()
    {
        ValidationResult = _validation.Validate(this);
        return ValidationResult.IsValid;
    }
}