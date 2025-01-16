using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.IntegrationTests.Extensions;
using CleanArchitecture.IntegrationTests.Fixtures;
using Shouldly;

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

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<TenantViewModel>();

        message?.Data.ShouldNotBeNull();

        message!.Data!.Id.ShouldBe(_fixture.CreatedTenantId);
        message.Data.Name.ShouldBe("Test Tenant");

        message.Data.Users.Count().ShouldBe(1);
    }

    [Test, Order(1)]
    public async Task Should_Get_All_Tenants()
    {
        var response = await _fixture.ServerClient.GetAsync(
            "api/v1/Tenant?searchTerm=Test&pageSize=5&page=1");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<PagedResult<TenantViewModel>>();

        message?.Data!.Items.ShouldNotBeEmpty();
        message!.Data!.Items.ShouldHaveSingleItem();
        message.Data!.Items
            .FirstOrDefault(x => x.Id == _fixture.CreatedTenantId)
            .ShouldNotBeNull();

        message.Data.Items
            .FirstOrDefault(x => x.Id == _fixture.CreatedTenantId)!
            .Users.Count().ShouldBe(1);
    }
    
    [Test, Order(2)]
    public async Task Should_Not_Get_Deleted_Tenant_By_Id()
    {
        var response = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.DeletedTenantId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var message = await response.Content.ReadAsJsonAsync<TenantViewModel>();

        message?.Data.ShouldBeNull();
    }
    
    [Test, Order(3)]
    public async Task Should_Get_All_Tenants_Including_Deleted()
    {
        var response = await _fixture.ServerClient.GetAsync(
            "api/v1/Tenant?searchTerm=Test&pageSize=5&page=1&includeDeleted=true");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<PagedResult<TenantViewModel>>();

        message?.Data!.Items.ShouldNotBeEmpty();
        message!.Data!.Items.Count.ShouldBe(2);
        message.Data!.Items
            .FirstOrDefault(x => x.Id == _fixture.DeletedTenantId)
            .ShouldNotBeNull();
    }

    [Test, Order(4)]
    public async Task Should_Create_Tenant()
    {
        var request = new CreateTenantViewModel("Test Tenant 2");

        var response = await _fixture.ServerClient.PostAsJsonAsync("/api/v1/Tenant", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<Guid>();
        var tenantId = message?.Data;

        // Check if tenant exists
        var tenantResponse = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{tenantId}");

        tenantResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var tenantMessage = await tenantResponse.Content.ReadAsJsonAsync<TenantViewModel>();

        tenantMessage?.Data.ShouldNotBeNull();

        tenantMessage!.Data!.Id.ShouldBe(tenantId!.Value);
        tenantMessage.Data.Name.ShouldBe(request.Name);
    }

    [Test, Order(5)]
    public async Task Should_Update_Tenant()
    {
        var request = new UpdateTenantViewModel(_fixture.CreatedTenantId, "Test Tenant 3");

        var response = await _fixture.ServerClient.PutAsJsonAsync("/api/v1/Tenant", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UpdateTenantViewModel>();

        message?.Data.ShouldNotBeNull();
        message!.Data.ShouldBeEquivalentTo(request);

        // Check if tenant is updated
        var tenantResponse = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        tenantResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var tenantMessage = await response.Content.ReadAsJsonAsync<TenantViewModel>();

        tenantMessage?.Data.ShouldNotBeNull();

        tenantMessage!.Data!.Id.ShouldBe(_fixture.CreatedTenantId);
        tenantMessage.Data.Name.ShouldBe(request.Name);
    }

    [Test, Order(6)]
    public async Task Should_Delete_Tenant()
    {
        var response = await _fixture.ServerClient.DeleteAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // Check if tenant is deleted
        var tenantResponse = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");

        tenantResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}