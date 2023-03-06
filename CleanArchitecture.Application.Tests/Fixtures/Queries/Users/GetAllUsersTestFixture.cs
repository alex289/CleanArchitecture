using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Moq;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Users;

public sealed class GetAllUsersTestFixture : QueryHandlerBaseFixture
{
    public Mock<IUserRepository> UserRepository { get; }
    public GetAllUsersQueryHandler Handler { get; }

    public GetAllUsersTestFixture()
    {
        UserRepository = new();
        
        Handler = new(UserRepository.Object);
    }
}