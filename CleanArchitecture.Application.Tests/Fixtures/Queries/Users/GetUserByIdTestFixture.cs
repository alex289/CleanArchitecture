using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Moq;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Users;

public sealed class GetUserByIdTestFixture : QueryHandlerBaseFixture
{
    public Mock<IUserRepository> UserRepository { get; }
    public GetUserByIdQueryHandler Handler { get; }

    public GetUserByIdTestFixture()
    {
        UserRepository = new();
        
        Handler = new(UserRepository.Object, Bus.Object);
    }
}