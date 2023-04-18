using System;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class User : Entity
{
    public User(
        Guid id,
        string email,
        string firstname,
        string lastName,
        string password,
        UserRole role) : base(id)
    {
        Email = email;
        FirstName = firstname;
        LastName = lastName;
        Password = password;
        Role = role;
    }

    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }

    public string FullName => $"{FirstName}, {LastName}";

    [MemberNotNull(nameof(Email))]
    public void SetEmail(string email)
    {
        if (email == null)
        {
            throw new ArgumentNullException(nameof(email));
        }

        if (email.Length > 320)
        {
            throw new ArgumentException(
                "Email may not be longer than 320 characters.");
        }

        Email = email;
    }

    [MemberNotNull(nameof(FirstName))]
    public void SetFirstName(string firstName)
    {
        if (firstName == null)
        {
            throw new ArgumentNullException(nameof(firstName));
        }

        if (firstName.Length > 100)
        {
            throw new ArgumentException(
                "First name may not be longer than 100 characters");
        }

        FirstName = firstName;
    }

    [MemberNotNull(nameof(LastName))]
    public void SetLastName(string lastName)
    {
        if (lastName == null)
        {
            throw new ArgumentNullException(nameof(lastName));
        }

        if (lastName.Length > 100)
        {
            throw new ArgumentException(
                "Last name may not be longer than 100 characters");
        }

        LastName = lastName;
    }

    [MemberNotNull(nameof(Password))]
    public void SetPassword(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        if (password.Length > 100)
        {
            throw new ArgumentException(
                "Password may not be longer than 100 characters");
        }

        Password = password;
    }

    public void SetRole(UserRole role)
    {
        Role = role;
    }
}