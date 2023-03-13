using System;
using System.Linq;
using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Users;

public sealed class GetAllUsersTestFixture : QueryHandlerBaseFixture
{
    private Mock<IUserRepository> UserRepository { get; }
    public GetAllUsersQueryHandler Handler { get; }
    public Guid ExistingUserId { get; } = Guid.NewGuid();

    public GetAllUsersTestFixture()
    {
        UserRepository = new();

        Handler = new(UserRepository.Object);
    }
    
    public void SetupUserAsync()
    {
        var user = new Mock<User>(() =>
                new User(
                    ExistingUserId, 
                    "max@mustermann.com", 
                    "Max", 
                    "Mustermann"));

        var query = new[] { user.Object }.AsQueryable().BuildMock();
        
        UserRepository
            .Setup(x => x.GetAllNoTracking())
            .Returns(query);
    }
}