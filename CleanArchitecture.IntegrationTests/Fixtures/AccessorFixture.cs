using System;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Infrastructure;
using Xunit;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public sealed class AccessorFixture : IAsyncLifetime
{
    public static string TestRunDbName { get; } = $"CleanArchitecture-Integration-{Guid.NewGuid()}";

    public async Task DisposeAsync()
    {
        var db = DatabaseAccessor.GetOrCreateAsync(TestRunDbName);
        await db.DisposeAsync();

        var redis = RedisAccessor.GetOrCreateAsync();
        await redis.DisposeAsync();

        var rabbit = RabbitmqAccessor.GetOrCreateAsync();
        await rabbit.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        var db = DatabaseAccessor.GetOrCreateAsync(TestRunDbName);
        await db.InitializeAsync();

        var redis = RedisAccessor.GetOrCreateAsync();
        await redis.InitializeAsync();

        var rabbit = RabbitmqAccessor.GetOrCreateAsync();
        await rabbit.InitializeAsync();
    }
}