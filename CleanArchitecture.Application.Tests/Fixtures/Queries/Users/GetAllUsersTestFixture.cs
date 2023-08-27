using System;
using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Users;

public sealed class GetAllUsersTestFixture : QueryHandlerBaseFixture
{
    public GetAllUsersTestFixture()
    {
        UserRepository = Substitute.For<IUserRepository>();

        Handler = new GetAllUsersQueryHandler(UserRepository);
    }

    private IUserRepository UserRepository { get; }
    public GetAllUsersQueryHandler Handler { get; }
    public Guid ExistingUserId { get; } = Guid.NewGuid();

    public void SetupUserAsync()
    {
        var user = new User(
                ExistingUserId,
                Guid.NewGuid(),
                "max@mustermann.com",
                "Max",
                "Mustermann",
                "Password",
                UserRole.User);

        var query = new[] { user }.BuildMock();

        UserRepository.GetAllNoTracking().Returns(query);
    }

    public void SetupDeletedUserAsync()
    {
        var user = new User(
                ExistingUserId,
                Guid.NewGuid(),
                "max@mustermann.com",
                "Max",
                "Mustermann",
                "Password",
                UserRole.User);

        user.Delete();

        var query = new[] { user }.BuildMock();

        UserRepository.GetAllNoTracking().Returns(query);
    }
}