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
        IServiceCollection services);

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
        
        var redisPort = GlobalSetupFixture.RedisContainer.GetMappedPublicPort(Configuration.RedisPort);
        var rabbitPort = GlobalSetupFixture.RabbitContainer.GetMappedPublicPort(Configuration.RabbitMqPort);

        Environment.SetEnvironmentVariable("ConnectionStrings:DefaultConnection", GlobalSetupFixture.DatabaseConnectionString);
        Environment.SetEnvironmentVariable("RedisHostName", $"localhost:{redisPort}");
        Environment.SetEnvironmentVariable("RabbitMQ:Port", rabbitPort.ToString());

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

            _registerCustomServicesHandler?.Invoke(services);
        });
    }
}