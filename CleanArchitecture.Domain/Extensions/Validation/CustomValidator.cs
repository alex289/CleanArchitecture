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
        return base64.Length % 4 == 0 && Base64Regex().IsMatch(base64);
    }

    public static IRuleBuilder<T, string> Password<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int minLength = 8,
        int maxLength = 50)
    {
        var options = ruleBuilder
            .NotEmpty().WithErrorCode(DomainErrorCodes.User.UserEmptyPassword)
            .MinimumLength(minLength).WithErrorCode(DomainErrorCodes.User.UserShortPassword)
            .MaximumLength(maxLength).WithErrorCode(DomainErrorCodes.User.UserLongPassword)
            .Matches("[A-Z]").WithErrorCode(DomainErrorCodes.User.UserUppercaseLetterPassword)
            .Matches("[a-z]").WithErrorCode(DomainErrorCodes.User.UserLowercaseLetterPassword)
            .Matches("[0-9]").WithErrorCode(DomainErrorCodes.User.UserNumberPassword)
            .Matches("[^a-zA-Z0-9]").WithErrorCode(DomainErrorCodes.User.UserSpecialCharPassword);
        return options;
    }

    [GeneratedRegex("^[a-zA-Z0-9\\+/]*={0,3}$", RegexOptions.None)]
    private static partial Regex Base64Regex();
}