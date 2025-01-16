using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.IntegrationTests.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shouldly;

namespace CleanArchitecture.IntegrationTests.ExternalServices;

public sealed class RedisTests
{
    private readonly RedisTestFixture _fixture = new();
    
    [OneTimeSetUp]
    public async Task Setup() => await _fixture.SeedTestData();
    
    [Test, Order(0)]
    public async Task Should_Get_Tenant_By_Id_And_Ensure_Cache()
    {
        var response = await _fixture.ServerClient.GetAsync($"/api/v1/Tenant/{_fixture.CreatedTenantId}");
        var message = await response.Content.ReadAsJsonAsync<TenantViewModel>();
        message!.Data!.Id.ShouldBe(_fixture.CreatedTenantId);

        var json = await _fixture.DistributedCache.GetStringAsync(CacheKeyGenerator.GetEntityCacheKey<Tenant>(_fixture.CreatedTenantId));
        json.ShouldNotBeNullOrEmpty();
        
        var tenant = JsonConvert.DeserializeObject<TenantViewModel>(json!)!;

        tenant.ShouldNotBeNull();
        tenant.Id.ShouldBe(_fixture.CreatedTenantId);
    }
}