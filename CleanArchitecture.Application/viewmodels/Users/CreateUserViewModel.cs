namespace CleanArchitecture.Application.ViewModels.Users;

public sealed record CreateUserViewModel(
    string Email,
    string Surname,
    string GivenName);