using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.gRPC.Tests.Fixtures;
using CleanArchitecture.Proto.Users;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.gRPC.Tests.Users;

public sealed class GetUsersByIdsTests : IClassFixture<UserTestsFixture>
{
    private readonly UserTestsFixture _fixture;

    public GetUsersByIdsTests(UserTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_Empty_List_If_No_Ids_Are_Given()
    {
        var result = await _fixture.UsersApiImplementation.GetByIds(
            SetupRequest(Enumerable.Empty<Guid>()),
            null!);

        result.Users.Should().HaveCount(0);
    }

    [Fact]
    public async Task Should_Get_Requested_Asked_Ids()
    {
        var nonExistingId = Guid.NewGuid();

        var ids = _fixture.ExistingUsers
            .Take(2)
            .Select(user => user.Id)
            .ToList();

        ids.Add(nonExistingId);

        var result = await _fixture.UsersApiImplementation.GetByIds(
            SetupRequest(ids),
            null!);

        result.Users.Should().HaveCount(2);

        foreach (var user in result.Users)
        {
            var userId = Guid.Parse(user.Id);

            userId.Should().NotBe(nonExistingId);

            var mockUser = _fixture.ExistingUsers.First(u => u.Id == userId);

            mockUser.Should().NotBeNull();

            user.Email.Should().Be(mockUser.Email);
            user.FirstName.Should().Be(mockUser.GivenName);
            user.LastName.Should().Be(mockUser.Surname);
        }
    }

    private static GetByIdsRequest SetupRequest(IEnumerable<Guid> ids)
    {
        var request = new GetByIdsRequest();

        request.Ids.AddRange(ids.Select(id => id.ToString()));
        request.Ids.Add("Not a guid");

        return request;
    }
}