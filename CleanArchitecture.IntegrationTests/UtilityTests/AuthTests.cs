using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures;
using Shouldly;

namespace CleanArchitecture.IntegrationTests.UtilityTests;

public sealed class AuthTests
{
    private readonly AuthTestFixure _fixture = new();

    [OneTimeSetUp]
    public async Task Setup() => await GlobalSetupFixture.RespawnDatabaseAsync();

    [Test]
    [TestCase("/api/v1/user")]
    [TestCase("/api/v1/user/me")]
    [TestCase("/api/v1/user/d74b112a-ece0-443d-9b4f-85bc418822ca")]
    [TestCase("/api/v1/tenant")]
    [TestCase("/api/v1/tenant/d74b112a-ece0-443d-9b4f-85bc418822ca")]
    public async Task Should_Get_Unauthorized_If_Trying_To_Call_Endpoint_Without_Token(string url)
    {
        var response = await _fixture.ServerClient.GetAsync(url);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}