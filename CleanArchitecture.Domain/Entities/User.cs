using System;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }

    public string FullName => $"{FirstName}, {LastName}";

    public Guid TenantId { get; private set; }
    public virtual Tenant Tenant { get; private set; } = null!;

    public User(
        Guid id,
        Guid tenantId,
        string email,
        string firstName,
        string lastName,
        string password,
        UserRole role) : base(id)
    {
        Email = email;
        TenantId = tenantId;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
    }

    public void SetEmail(string email)
    {
        Email = email;
    }

    public void SetFirstName(string firstName)
    {
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        LastName = lastName;
    }

    public void SetPassword(string password)
    {
        Password = password;
    }

    public void SetRole(UserRole role)
    {
        Role = role;
    }
}