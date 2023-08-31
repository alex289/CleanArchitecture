using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.gRPC.Tests.Fixtures;
using CleanArchitecture.Proto.Users;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.gRPC.Tests.Users;

public sealed class GetUsersByIdsTests : IClassFixture<UserTestFixture>
{
    private readonly UserTestFixture _fixture;

    public GetUsersByIdsTests(UserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_Empty_List_If_No_Ids_Are_Given()
    {
        var result = await _fixture.UsersApiImplementation.GetByIds(
            SetupRequest(Enumerable.Empty<Guid>()),
            default!);

        result.Users.Should().HaveCount(0);
    }

    [Fact]
    public async Task Should_Get_Requested_Users()
    {
        var nonExistingId = Guid.NewGuid();

        var ids = _fixture.ExistingUsers
            .Take(2)
            .Select(user => user.Id)
            .ToList();

        ids.Add(nonExistingId);

        var result = await _fixture.UsersApiImplementation.GetByIds(
            SetupRequest(ids),
            default!);

        result.Users.Should().HaveCount(2);

        foreach (var user in result.Users)
        {
            var userId = Guid.Parse(user.Id);

            userId.Should().NotBe(nonExistingId);

            var mockUser = _fixture.ExistingUsers.First(u => u.Id == userId);

            mockUser.Should().NotBeNull();

            user.Email.Should().Be(mockUser.Email);
            user.FirstName.Should().Be(mockUser.FirstName);
            user.LastName.Should().Be(mockUser.LastName);
        }
    }

    private static GetUsersByIdsRequest SetupRequest(IEnumerable<Guid> ids)
    {
        var request = new GetUsersByIdsRequest();

        request.Ids.AddRange(ids.Select(id => id.ToString()));
        request.Ids.Add("Not a guid");

        return request;
    }
}