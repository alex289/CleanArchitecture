using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures.gRPC;
using CleanArchitecture.Proto.Users;
using Shouldly;

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

        response.Users.ShouldHaveSingleItem();

        var user = response.Users.First();
        var createdUser = _fixture.CreateUser();

        user.Email.ShouldBe(createdUser.Email);
        user.FirstName.ShouldBe(createdUser.FirstName);
        user.LastName.ShouldBe(createdUser.LastName);
        user.DeletedAt.ShouldNotBeNullOrWhiteSpace();
    }
}