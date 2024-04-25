using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace CleanArchitecture.IntegrationTests.Infrastructure;

public sealed class RedisAccessor
{
    private static readonly ConcurrentDictionary<string, RedisAccessor> s_accessors = new();

    private static readonly RedisContainer s_redisContainer = new RedisBuilder()
        .WithPortBinding(6379)
        .Build();

    public async Task InitializeAsync()
    {
        await s_redisContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await s_redisContainer.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return s_redisContainer.GetConnectionString();
    }

    public void RegisterRedis(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var distributedCache = serviceCollection.FirstOrDefault(x =>
            x.ServiceType == typeof(IDistributedCache));

        if (distributedCache != null)
        {
            serviceCollection.Remove(distributedCache);
        }

        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["RedisHostName"];
            options.InstanceName = "clean-architecture";
        });
    }
    
    public void ResetRedis()
    {
        var redis = ConnectionMultiplexer.Connect(GetConnectionString());

        var db = redis.GetDatabase();

        var endpoints = redis.GetEndPoints();
        foreach (var endpoint in endpoints)
        {
            var server = redis.GetServer(endpoint);
            var keys = server.Keys();
            foreach (var key in keys)
            {
                db.KeyDelete(key);
            }
        }

        redis.Close();
    }

    public static RedisAccessor GetOrCreateAsync()
    {
        return s_accessors.GetOrAdd("redis", _ => new RedisAccessor());
    }
}