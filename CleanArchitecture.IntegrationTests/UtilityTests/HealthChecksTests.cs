using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Priority;

namespace CleanArchitecture.IntegrationTests.UtilityTests;

[Collection("IntegrationTests")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public sealed class HealthChecksTests : IClassFixture<AuthTestFixure>
{
    private readonly AuthTestFixure _fixture;

    public HealthChecksTests(AuthTestFixure fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Priority(0)]
    public async Task Should_Return_Healthy()
    {
        var response = await _fixture.ServerClient.GetAsync("/healthz");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(content);
        json["status"]!.Value<string>().Should().Be(HealthStatus.Healthy.ToString());
    }
}