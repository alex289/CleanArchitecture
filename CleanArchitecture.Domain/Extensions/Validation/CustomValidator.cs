using System.Text.RegularExpressions;
using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Extensions.Validation;

public static class CustomValidator
{
    public static IRuleBuilderOptions<T, string> StringMustBeBase64<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(x => IsBase64String(x));
    }

    private static bool IsBase64String(string base64)
    {
        base64 = base64.Trim();
        return base64.Length % 4 == 0 && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }

    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength = 8, int maxLength = 50)
    {
        var options = ruleBuilder
            .NotEmpty().WithErrorCode(DomainErrorCodes.UserEmptyPassword)
            .MinimumLength(minLength).WithErrorCode(DomainErrorCodes.UserShortPassword)
            .MaximumLength(maxLength).WithErrorCode(DomainErrorCodes.UserLongPassword)
            .Matches("[A-Z]").WithErrorCode(DomainErrorCodes.UserUppercaseLetterPassword)
            .Matches("[a-z]").WithErrorCode(DomainErrorCodes.UserLowercaseLetterPassword)
            .Matches("[0-9]").WithErrorCode(DomainErrorCodes.UserNumberPassword)
            .Matches("[^a-zA-Z0-9]").WithErrorCode(DomainErrorCodes.UserSpecialCharPassword);
        return options;
    }
}