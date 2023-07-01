using System;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Interfaces;

public interface IUser
{
    string Name { get; }
    Guid GetUserId();
    UserRole GetUserRole();
    string GetUserEmail();
}