using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Users;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Application.Tests.Queries.Users;

public sealed class GetAllUsersQueryHandlerTests
{
    private readonly GetAllUsersTestFixture _fixture = new();

    [Fact]
    public async Task Should_Get_All_Users()
    {
        _fixture.SetupUserAsync();

        var result = await _fixture.Handler.Handle(
            new GetAllUsersQuery(),
            default);

        _fixture.VerifyNoDomainNotification();

        var userViewModels = result.ToArray();
        userViewModels.Should().NotBeNull();
        userViewModels.Should().ContainSingle();
        userViewModels.FirstOrDefault()!.Id.Should().Be(_fixture.ExistingUserId);
    }

    [Fact]
    public async Task Should_Not_Get_Deleted_Users()
    {
        _fixture.SetupDeletedUserAsync();

        var result = await _fixture.Handler.Handle(
            new GetAllUsersQuery(),
            default);

        _fixture.VerifyNoDomainNotification();

        result.Should().BeEmpty();
    }
}