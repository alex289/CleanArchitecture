using System;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Interfaces;

public interface IUser
{
    Guid GetUserId();
    UserRole GetUserRole();

    string Name { get; }
}
