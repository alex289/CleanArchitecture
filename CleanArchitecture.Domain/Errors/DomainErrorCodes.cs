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
    
    // User
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
}