using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;

namespace CleanArchitecture.IntegrationTests.UtilityTests;

public sealed class HealthChecksTests
{
    private readonly AuthTestFixure _fixture = new();

    [OneTimeSetUp]
    public async Task Setup() => await GlobalSetupFixture.RespawnDatabaseAsync();

    [Test, Order(0)]
    public async Task Should_Return_Healthy()
    {
        var response = await _fixture.ServerClient.GetAsync("/healthz");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(content);
        json["status"]!.Value<string>().Should().Be(HealthStatus.Healthy.ToString());
    }
}