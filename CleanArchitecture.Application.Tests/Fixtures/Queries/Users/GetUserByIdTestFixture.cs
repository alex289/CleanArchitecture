using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable.Moq;
using Moq;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Users;

public sealed class GetUserByIdTestFixture : QueryHandlerBaseFixture
{
    public Mock<IUserRepository> UserRepository { get; }
    public GetUserByIdQueryHandler Handler { get; }
    public Guid ExistingUserId { get; } = Guid.NewGuid();

    public GetUserByIdTestFixture()
    {
        UserRepository = new();
        
        SetupUserRepositoryGetAllAsync();
        
        Handler = new(UserRepository.Object, Bus.Object);
    }
    
    private void SetupUserRepositoryGetAllAsync()
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