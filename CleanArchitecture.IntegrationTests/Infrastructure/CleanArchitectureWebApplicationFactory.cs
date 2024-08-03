using System;
using System.Collections.Generic;
using CleanArchitecture.IntegrationTests.Constants;
using CleanArchitecture.IntegrationTests.Infrastructure.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Infrastructure;

public sealed class CleanArchitectureWebApplicationFactory : WebApplicationFactory<Program>
{
    public delegate void RegisterCustomServicesHandler(
        IServiceCollection services,
        ServiceProvider serviceProvider,
        IServiceProvider scopedServices);

    private readonly bool _addTestAuthentication;
    private readonly RegisterCustomServicesHandler? _registerCustomServicesHandler;

    public CleanArchitectureWebApplicationFactory(
        RegisterCustomServicesHandler? registerCustomServicesHandler,
        bool addTestAuthentication)
    {
        _registerCustomServicesHandler = registerCustomServicesHandler;
        _addTestAuthentication = addTestAuthentication;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Integration");

        base.ConfigureWebHost(builder);

        builder.ConfigureAppConfiguration(configuration =>
        {
            var redisPort = GlobalSetupFixture.RedisContainer.GetMappedPublicPort(Configuration.RedisPort);
            var rabbitPort = GlobalSetupFixture.RabbitContainer.GetMappedPublicPort(Configuration.RabbitMqPort);

            configuration.AddInMemoryCollection([
                new KeyValuePair<string, string?>(
                    "ConnectionStrings:DefaultConnection",
                    GlobalSetupFixture.DatabaseConnectionString),
                new KeyValuePair<string, string?>(
                    "RedisStackExchange:RedisConfigString",
                    $"localhost:{redisPort},abortConnect=true"),
                new KeyValuePair<string, string?>(
                    "RabbitMQ:Host",
                    $"localhost:{rabbitPort}")
            ]);
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
            _registerCustomServicesHandler?.Invoke(services, sp, scopedServices);
        });
    }
}