using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures.gRPC;
using CleanArchitecture.Proto.Users;
using FluentAssertions;
using Xunit;
using Xunit.Priority;

namespace CleanArchitecture.IntegrationTests.gRPC;

[Collection("IntegrationTests")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public sealed class GetUsersByIdsTests : IClassFixture<GetUsersByIdsTestFixture>
{
    private readonly GetUsersByIdsTestFixture _fixture;

    public GetUsersByIdsTests(GetUsersByIdsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_Users_By_Ids()
    {
        var client = new UsersApi.UsersApiClient(_fixture.GrpcChannel);

        var request = new GetUsersByIdsRequest();
        request.Ids.Add(_fixture.CreatedUserId.ToString());

        var response = await client.GetByIdsAsync(request);

        response.Users.Should().HaveCount(1);

        var user = response.Users.First();
        var createdUser = _fixture.CreateUser();

        user.Email.Should().Be(createdUser.Email);
        user.FirstName.Should().Be(createdUser.FirstName);
        user.LastName.Should().Be(createdUser.LastName);
    }
}