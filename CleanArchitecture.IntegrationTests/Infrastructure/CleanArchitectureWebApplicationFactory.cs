using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.IntegrationTests.Infrastructure.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Infrastructure;

public sealed class CleanArchitectureWebApplicationFactory : WebApplicationFactory<Program>
{
    public delegate void AddCustomSeedDataHandler(ApplicationDbContext context);

    public delegate void RegisterCustomServicesHandler(
        IServiceCollection services,
        ServiceProvider serviceProvider,
        IServiceProvider scopedServices);

    private readonly string _instanceDatabaseName;

    private readonly bool _addTestAuthentication;
    private readonly RegisterCustomServicesHandler? _registerCustomServicesHandler;

    public CleanArchitectureWebApplicationFactory(
        RegisterCustomServicesHandler? registerCustomServicesHandler,
        bool addTestAuthentication,
        string instanceDatabaseName)
    {
        _registerCustomServicesHandler = registerCustomServicesHandler;
        _addTestAuthentication = addTestAuthentication;
        _instanceDatabaseName = instanceDatabaseName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Integration");

        base.ConfigureWebHost(builder);

        var configuration = new ConfigurationBuilder()
            .Build();

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddEnvironmentVariables();

            var dbAccessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
            var redisAccessor = RedisAccessor.GetOrCreateAsync();
            var rabbitAccessor = RabbitmqAccessor.GetOrCreateAsync();

            // Overwrite default connection strings
            configurationBuilder.AddInMemoryCollection([
                new KeyValuePair<string, string?>(
                    "ConnectionStrings:DefaultConnection",
                    dbAccessor.GetConnectionString()),
                new KeyValuePair<string, string?>(
                    "RedisHostName",
                    redisAccessor.GetConnectionString()),
                new KeyValuePair<string, string?>(
                    "RabbitMQ:Host",
                    rabbitAccessor.GetConnectionString())
            ]);

            configuration = configurationBuilder.Build();
        });

        builder.ConfigureServices(services =>
        {
            if (_addTestAuthentication)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Testing";
                    options.DefaultChallengeScheme = "Testing";
                }).AddTestAuthentication(_ => { });
            }

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var dbAccessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
            dbAccessor.CreateDatabase(scopedServices);

            var redisAccessor = RedisAccessor.GetOrCreateAsync();
            redisAccessor.RegisterRedis(services, configuration);

            var rabbitAccessor = RabbitmqAccessor.GetOrCreateAsync();
            rabbitAccessor.RegisterRabbitmq(services, configuration);

            _registerCustomServicesHandler?.Invoke(services, sp, scopedServices);
        });
    }

    public async Task RespawnDatabaseAsync()
    {
        var dbAccessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
        await dbAccessor.RespawnDatabaseAsync();
        
        var redisAccessor = RedisAccessor.GetOrCreateAsync();
        redisAccessor.ResetRedis();
    }

    public override async ValueTask DisposeAsync()
    {
        var dbAccessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
        await dbAccessor.DisposeAsync();

        var redisAccessor = RedisAccessor.GetOrCreateAsync();
        await redisAccessor.DisposeAsync();

        var rabbitAccessor = RabbitmqAccessor.GetOrCreateAsync();
        await rabbitAccessor.DisposeAsync();
    }
}