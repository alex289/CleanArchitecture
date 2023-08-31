using System;

namespace CleanArchitecture.Domain.Commands.Users.ChangePassword;

public sealed class ChangePasswordCommand : CommandBase
{
    private static readonly ChangePasswordCommandValidation s_validation = new();

    public string Password { get; }
    public string NewPassword { get; }

    public ChangePasswordCommand(string password, string newPassword) : base(Guid.NewGuid())
    {
        Password = password;
        NewPassword = newPassword;
    }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}