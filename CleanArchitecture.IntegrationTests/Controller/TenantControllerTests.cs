using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.IntegrationTests.Extensions;
using CleanArchitecture.IntegrationTests.Fixtures;
using FluentAssertions;

namespace CleanArchitecture.IntegrationTests.Controller;

public sealed class TenantControllerTests
{
    private readonly TenantTestFixture _fixture = new();

    [OneTimeSetUp]
    public async Task Setup() => await _fixture.SeedTestData();

    [Test, Order(0)]
    public async Task Should_Get_Tenant_By_Id()
    {
        var response = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<TenantViewModel>();

        message?.Data.Should().NotBeNull();

        message!.Data!.Id.Should().Be(_fixture.CreatedTenantId);
        message.Data.Name.Should().Be("Test Tenant");

        message.Data.Users.Count().Should().Be(1);
    }

    [Test, Order(1)]
    public async Task Should_Get_All_Tenants()
    {
        var response = await _fixture.ServerClient.GetAsync(
            "api/v1/Tenant?searchTerm=Test&pageSize=5&page=1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<PagedResult<TenantViewModel>>();

        message?.Data!.Items.Should().NotBeEmpty();
        message!.Data!.Items.Should().HaveCount(1);
        message.Data!.Items
            .FirstOrDefault(x => x.Id == _fixture.CreatedTenantId)
            .Should().NotBeNull();

        message.Data.Items
            .FirstOrDefault(x => x.Id == _fixture.CreatedTenantId)!
            .Users.Count().Should().Be(1);
    }

    [Test, Order(2)]
    public async Task Should_Create_Tenant()
    {
        var request = new CreateTenantViewModel("Test Tenant 2");

        var response = await _fixture.ServerClient.PostAsJsonAsync("/api/v1/Tenant", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<Guid>();
        var tenantId = message?.Data;

        // Check if tenant exists
        var tenantResponse = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{tenantId}");

        tenantResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tenantMessage = await tenantResponse.Content.ReadAsJsonAsync<TenantViewModel>();

        tenantMessage?.Data.Should().NotBeNull();

        tenantMessage!.Data!.Id.Should().Be(tenantId!.Value);
        tenantMessage.Data.Name.Should().Be(request.Name);
    }

    [Test, Order(3)]
    public async Task Should_Update_Tenant()
    {
        var request = new UpdateTenantViewModel(_fixture.CreatedTenantId, "Test Tenant 3");

        var response = await _fixture.ServerClient.PutAsJsonAsync("/api/v1/Tenant", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UpdateTenantViewModel>();

        message?.Data.Should().NotBeNull();
        message!.Data.Should().BeEquivalentTo(request);

        // Check if tenant is updated
        var tenantResponse = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        tenantResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tenantMessage = await response.Content.ReadAsJsonAsync<TenantViewModel>();

        tenantMessage?.Data.Should().NotBeNull();

        tenantMessage!.Data!.Id.Should().Be(_fixture.CreatedTenantId);
        tenantMessage.Data.Name.Should().Be(request.Name);
    }

    [Test, Order(4)]
    public async Task Should_Delete_Tenant()
    {
        var response = await _fixture.ServerClient.DeleteAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Check if tenant is deleted
        var tenantResponse = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        tenantResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}