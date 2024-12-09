using System;

namespace CleanArchitecture.Shared.Users;

public sealed record UserViewModel(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    DateTimeOffset? DeletedAt);