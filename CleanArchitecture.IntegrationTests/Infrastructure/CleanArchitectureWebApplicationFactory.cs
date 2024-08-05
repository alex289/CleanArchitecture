using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Rabbitmq;
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

        var configuration = new ConfigurationBuilder()
            .Build();

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var redisPort = GlobalSetupFixture.RedisContainer.GetMappedPublicPort(Configuration.RedisPort);
            var rabbitPort = GlobalSetupFixture.RabbitContainer.GetMappedPublicPort(Configuration.RabbitMqPort);

            configurationBuilder.AddInMemoryCollection([
                new KeyValuePair<string, string?>(
                    "ConnectionStrings:DefaultConnection",
                    GlobalSetupFixture.DatabaseConnectionString),
                new KeyValuePair<string, string?>(
                    "RedisHostName",
                    $"localhost:{redisPort}"),
                new KeyValuePair<string, string?>(
                    "RabbitMQ:Port",
                    rabbitPort.ToString())
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

            // Readd rabbitmq options to use the correct port
            var rabbitMq = new RabbitMqConfiguration();
            configuration.Bind("RabbitMQ", rabbitMq);
            services.AddSingleton(rabbitMq);

            // Readd IDistributedCache to replace the memory cache with redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisHostName"];
                options.InstanceName = "clean-architecture";
            });

            _registerCustomServicesHandler?.Invoke(services, sp, scopedServices);
        });
    }
}