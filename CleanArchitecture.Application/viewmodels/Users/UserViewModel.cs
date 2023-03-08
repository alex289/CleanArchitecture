using System;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ViewModels.Users;

public sealed class UserViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public static UserViewModel FromUser(User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            GivenName = user.GivenName,
            Surname = user.Surname
        };
    }
}
