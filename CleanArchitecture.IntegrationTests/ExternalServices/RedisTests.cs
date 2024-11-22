using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.IntegrationTests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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
        message!.Data!.Id.Should().Be(_fixture.CreatedTenantId);

        var json = await _fixture.DistributedCache.GetStringAsync(CacheKeyGenerator.GetEntityCacheKey<Tenant>(_fixture.CreatedTenantId));
        json.Should().NotBeNullOrEmpty();
        
        var tenant = JsonConvert.DeserializeObject<TenantViewModel>(json!)!;

        tenant.Should().NotBeNull();
        tenant.Id.Should().Be(_fixture.CreatedTenantId);
    }
}