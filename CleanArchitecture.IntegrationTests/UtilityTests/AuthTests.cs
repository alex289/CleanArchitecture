using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Fixtures;
using FluentAssertions;
using Xunit;
using Xunit.Priority;

namespace CleanArchitecture.IntegrationTests.UtilityTests;

[Collection("IntegrationTests")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public sealed class AuthTests : IClassFixture<AuthTestFixure>
{
    private readonly AuthTestFixure _fixture;

    public AuthTests(AuthTestFixure fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("/api/v1/user")]
    [InlineData("/api/v1/user/me")]
    [InlineData("/api/v1/user/d74b112a-ece0-443d-9b4f-85bc418822ca")]
    [InlineData("/api/v1/tenant")]
    [InlineData("/api/v1/tenant/d74b112a-ece0-443d-9b4f-85bc418822ca")]
    public async Task Should_Get_Unauthorized_If_Trying_To_Call_Endpoint_Without_Token(
        string url)
    {
        var response = await _fixture.ServerClient.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}