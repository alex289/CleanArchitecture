using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Users;
using CleanArchitecture.Application.ViewModels;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Application.Tests.Queries.Users;

public sealed class GetAllUsersQueryHandlerTests
{
    private readonly GetAllUsersTestFixture _fixture = new();

    [Fact]
    public async Task Should_Get_All_Users()
    {
        var user = _fixture.SetupUserAsync();

        var query = new PageQuery
        {
            PageSize = 1,
            Page = 1
        };

        var result = await _fixture.Handler.Handle(
            new GetAllUsersQuery(query, false, user.Email),
            default);

        _fixture.VerifyNoDomainNotification();

        result.PageSize.ShouldBe(query.PageSize);
        result.Page.ShouldBe(query.Page);
        result.Count.ShouldBe(1);

        var userViewModels = result.Items.ToArray();
        userViewModels.ShouldNotBeNull();
        userViewModels.ShouldHaveSingleItem();
        userViewModels.FirstOrDefault()!.Id.ShouldBe(_fixture.ExistingUserId);
    }

    [Fact]
    public async Task Should_Not_Get_Deleted_Users()
    {
        _fixture.SetupDeletedUserAsync();

        var query = new PageQuery
        {
            PageSize = 10,
            Page = 1
        };

        var result = await _fixture.Handler.Handle(
            new GetAllUsersQuery(query, false),
            default);

        _fixture.VerifyNoDomainNotification();

        result.PageSize.ShouldBe(query.PageSize);
        result.Page.ShouldBe(query.Page);
        result.Count.ShouldBe(0);

        result.Items.ShouldBeEmpty();
    }
}