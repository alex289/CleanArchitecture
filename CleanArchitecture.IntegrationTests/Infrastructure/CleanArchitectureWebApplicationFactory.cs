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

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddEnvironmentVariables();

            var accessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);

            // Overwrite default connection string to our test instance db
            configurationBuilder.AddInMemoryCollection([
                new KeyValuePair<string, string?>(
                    "ConnectionStrings:DefaultConnection",
                    accessor.GetConnectionString())
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

            var accessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
            var applicationDbContext = accessor.CreateDatabase(scopedServices);

            _registerCustomServicesHandler?.Invoke(services, sp, scopedServices);
        });
    }

    public async Task RespawnDatabaseAsync()
    {
        var accessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
        await accessor.RespawnDatabaseAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        var accessor = DatabaseAccessor.GetOrCreateAsync(_instanceDatabaseName);
        await accessor.DisposeAsync();
    }
}