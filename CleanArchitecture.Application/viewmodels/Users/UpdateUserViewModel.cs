using System;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.ViewModels.Users;

public sealed record UpdateUserViewModel(
    Guid Id,
    string Email,
    string Surname,
    string GivenName,
    UserRole Role);