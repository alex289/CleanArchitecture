using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Users;
using CleanArchitecture.Domain.Errors;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Application.Tests.Queries.Users;

public sealed class GetUserByIdQueryHandlerTests
{
    private readonly GetUserByIdTestFixture _fixture = new();

    [Fact]
    public async Task Should_Get_Existing_User()
    {
        _fixture.SetupUserAsync();

        var result = await _fixture.Handler.Handle(
            new GetUserByIdQuery(_fixture.ExistingUserId),
            default);

        _fixture.VerifyNoDomainNotification();

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(_fixture.ExistingUserId);
    }

    [Fact]
    public async Task Should_Raise_Notification_For_No_User()
    {
        _fixture.SetupUserAsync();

        var request = new GetUserByIdQuery(Guid.NewGuid());
        var result = await _fixture.Handler.Handle(
            request,
            default);

        _fixture.VerifyExistingNotification(
            nameof(GetUserByIdQuery),
            ErrorCodes.ObjectNotFound,
            $"User with id {request.Id} could not be found");

        result.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Get_Deleted_User()
    {
        _fixture.SetupDeletedUserAsync();

        var result = await _fixture.Handler.Handle(
            new GetUserByIdQuery(_fixture.ExistingUserId),
            default);

        _fixture.VerifyExistingNotification(
            nameof(GetUserByIdQuery),
            ErrorCodes.ObjectNotFound,
            $"User with id {_fixture.ExistingUserId} could not be found");

        result.ShouldBeNull();
    }
}