using System;

namespace CleanArchitecture.Application.ViewModels.Users;

public sealed record UpdateUserViewModel(
    Guid Id,
    string Email,
    string Surname,
    string GivenName);