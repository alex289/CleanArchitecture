using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Application.Tests.Fixtures.Queries.Users;
using CleanArchitecture.Domain.Errors;
using FluentAssertions;
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
            new(_fixture.ExistingUserId, false),
            default);
        
        _fixture.VerifyNoDomainNotification();

        result.Should().NotBeNull();
        result!.Id.Should().Be(_fixture.ExistingUserId);
    }
    
    [Fact]
    public async Task Should_Raise_Notification_For_No_User()
    {
        _fixture.SetupUserAsync();

        var request = new GetUserByIdQuery(Guid.NewGuid(), false);
        var result = await _fixture.Handler.Handle(
            request,
            default);
        
        _fixture.VerifyExistingNotification(
            nameof(GetUserByIdQuery),
            ErrorCodes.ObjectNotFound,
            $"User with id {request.UserId} could not be found");

        result.Should().BeNull();
    }

    // Add Test for deleted user
}