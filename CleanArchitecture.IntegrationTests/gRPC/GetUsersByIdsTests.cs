using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures.gRPC;
using CleanArchitecture.Proto.Users;
using FluentAssertions;

namespace CleanArchitecture.IntegrationTests.gRPC;

public sealed class GetUsersByIdsTests
{
    private readonly GetUsersByIdsTestFixture _fixture = new();

    [OneTimeSetUp]
    public async Task Setup() => await _fixture.SeedTestData();

    [Test]
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