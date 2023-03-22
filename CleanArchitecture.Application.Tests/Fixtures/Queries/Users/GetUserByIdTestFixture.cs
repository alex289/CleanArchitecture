using System;
using System.Linq;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Users;

public sealed class GetUserByIdTestFixture : QueryHandlerBaseFixture
{
    public GetUserByIdTestFixture()
    {
        UserRepository = new Mock<IUserRepository>();

        Handler = new GetUserByIdQueryHandler(UserRepository.Object, Bus.Object);
    }

    private Mock<IUserRepository> UserRepository { get; }
    public GetUserByIdQueryHandler Handler { get; }
    public Guid ExistingUserId { get; } = Guid.NewGuid();

    public void SetupUserAsync()
    {
        var user = new Mock<User>(() =>
            new User(
                ExistingUserId,
                "max@mustermann.com",
                "Max",
                "Mustermann",
                "Password",
                UserRole.User));

        var query = new[] { user.Object }.AsQueryable().BuildMock();

        UserRepository
            .Setup(x => x.GetAllNoTracking())
            .Returns(query);
    }

    public void SetupDeletedUserAsync()
    {
        var user = new Mock<User>(() =>
            new User(
                ExistingUserId,
                "max@mustermann.com",
                "Max",
                "Mustermann",
                "Password",
                UserRole.User));

        user.Object.Delete();

        var query = new[] { user.Object }.AsQueryable().BuildMock();

        UserRepository
            .Setup(x => x.GetAllNoTracking())
            .Returns(query);
    }
}