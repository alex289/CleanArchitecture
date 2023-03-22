using System;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class User : Entity
{
    public User(
        Guid id,
        string email,
        string surname,
        string givenName,
        string password,
        UserRole role) : base(id)
    {
        Email = email;
        GivenName = givenName;
        Surname = surname;
        Password = password;
        Role = role;
    }

    public string Email { get; private set; }
    public string GivenName { get; private set; }
    public string Surname { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }

    public string FullName => $"{Surname}, {GivenName}";

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

    [MemberNotNull(nameof(GivenName))]
    public void SetGivenName(string givenName)
    {
        if (givenName == null)
        {
            throw new ArgumentNullException(nameof(givenName));
        }

        if (givenName.Length > 100)
        {
            throw new ArgumentException(
                "Given name may not be longer than 100 characters");
        }

        GivenName = givenName;
    }

    [MemberNotNull(nameof(Surname))]
    public void SetSurname(string surname)
    {
        if (surname == null)
        {
            throw new ArgumentNullException(nameof(surname));
        }

        if (surname.Length > 100)
        {
            throw new ArgumentException(
                "Surname may not be longer than 100 characters");
        }

        Surname = surname;
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