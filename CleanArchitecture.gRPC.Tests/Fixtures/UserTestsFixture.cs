using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.gRPC.Tests.Fixtures;

public sealed class UserTestsFixture
{
    private Mock<IUserRepository> UserRepository { get; } = new ();

    public UsersApiImplementation UsersApiImplementation { get; }

    public IEnumerable<User> ExistingUsers { get; }

    public UserTestsFixture()
    {
        ExistingUsers = new List<User>()
        {
            new (
                Guid.NewGuid(), 
                "test@test.de", 
                "Test First Name", 
                "Test Last Name",
                "Test Password",
                UserRole.User),
            new (
                Guid.NewGuid(), 
                "email@Email.de", 
                "Email First Name", 
                "Email Last Name",
                "Email Password",
                UserRole.Admin),
            new (
                Guid.NewGuid(), 
                "user@user.de", 
                "User First Name", 
                "User Last Name",
                "User Password",
                UserRole.User),
        };

        var queryable = ExistingUsers.AsQueryable().BuildMock();

        UserRepository
            .Setup(repository => repository.GetAllNoTracking())
            .Returns(queryable);

        UsersApiImplementation = new UsersApiImplementation(UserRepository.Object);
    }
}