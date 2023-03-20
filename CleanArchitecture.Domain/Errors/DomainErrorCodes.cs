namespace CleanArchitecture.Domain.Errors;

public static class DomainErrorCodes
{
    // User Validation
    public const string UserEmptyId = "USER_EMPTY_ID";
    public const string UserEmptySurname = "USER_EMPTY_SURNAME";
    public const string UserEmptyGivenName = "USER_EMPTY_GIVEN_NAME";
    public const string UserEmailExceedsMaxLength = "USER_EMAIL_EXCEEDS_MAX_LENGTH";
    public const string UserSurnameExceedsMaxLength = "USER_SURNAME_EXCEEDS_MAX_LENGTH";
    public const string UserGivenNameExceedsMaxLength = "USER_GIVEN_NAME_EXCEEDS_MAX_LENGTH";
    public const string UserInvalidEmail = "USER_INVALID_EMAIL";
    
    // User Password Validation
    public const string UserEmptyPassword = "USER_PASSWORD_MAY_NOT_BE_EMPTY";
    public const string UserShortPassword = "USER_PASSWORD_MAY_NOT_BE_SHORTER_THAN_6_CHARACTERS";
    public const string UserLongPassword = "USER_PASSWORD_MAY_NOT_BE_LONGER_THAN_50_CHARACTERS";
    public const string UserUppercaseLetterPassword = "USER_PASSWORD_MUST_CONTAIN_A_UPPERCASE_LETTER";
    public const string UserLowercaseLetterPassword = "USER_PASSWORD_MUST_CONTAIN_A_LOWERCASE_LETTER";
    public const string UserNumberPassword = "USER_PASSWORD_MUST_CONTAIN_A_NUMBER";
    public const string UserSpecialCharPassword = "USER_PASSWORD_MUST_CONTAIN_A_SPECIAL_CHARACTER";
    
    // User
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
    public const string UserPasswordIncorrect = "USER_PASSWORD_INCORRECT";
}