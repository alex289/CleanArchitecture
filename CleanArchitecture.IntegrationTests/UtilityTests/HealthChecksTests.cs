using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Shouldly;

namespace CleanArchitecture.IntegrationTests.UtilityTests;

public sealed class HealthChecksTests
{
    private readonly AuthTestFixure _fixture = new();

    [OneTimeSetUp]
    public async Task Setup() => await GlobalSetupFixture.RespawnDatabaseAsync();

    [Test, Order(0)]
    public async Task Should_Return_Healthy()
    {
        // Wait some time to let the services get healthy
        await Task.Delay(2000);
        
        var response = await _fixture.ServerClient.GetAsync("/healthz");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(content);
        json["status"]!.Value<string>().ShouldBe(HealthStatus.Healthy.ToString());
    }
}