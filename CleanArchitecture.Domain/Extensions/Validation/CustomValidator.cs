using System.Text.RegularExpressions;
using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Extensions.Validation;

public static partial class CustomValidator
{
    public static IRuleBuilderOptions<T, string> StringMustBeBase64<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(x => IsBase64String(x));
    }

    private static bool IsBase64String(string base64)
    {
        base64 = base64.Trim();
        return base64.Length % 4 == 0 && new Regex("^[a-zA-Z0-9\\+/]*={0,3}$").IsMatch(base64);
    }

    public static IRuleBuilder<T, string> Password<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int minLength = 8,
        int maxLength = 50)
    {
        var options = ruleBuilder
            .NotEmpty().WithErrorCode(DomainErrorCodes.User.EmptyPassword)
            .MinimumLength(minLength).WithErrorCode(DomainErrorCodes.User.ShortPassword)
            .MaximumLength(maxLength).WithErrorCode(DomainErrorCodes.User.LongPassword)
            .Matches("[A-Z]").WithErrorCode(DomainErrorCodes.User.UppercaseLetterPassword)
            .Matches("[a-z]").WithErrorCode(DomainErrorCodes.User.LowercaseLetterPassword)
            .Matches("[0-9]").WithErrorCode(DomainErrorCodes.User.NumberPassword)
            .Matches("[^a-zA-Z0-9]").WithErrorCode(DomainErrorCodes.User.SpecialCharPassword);
        return options;
    }
}