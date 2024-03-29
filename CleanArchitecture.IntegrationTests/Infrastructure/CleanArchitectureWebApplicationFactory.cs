﻿using System;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.IntegrationTests.Extensions;
using CleanArchitecture.IntegrationTests.Infrastructure.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Infrastructure;

public sealed class CleanArchitectureWebApplicationFactory : WebApplicationFactory<Program>
{
    public delegate void AddCustomSeedDataHandler(ApplicationDbContext context);

    public delegate void RegisterCustomServicesHandler(
        IServiceCollection services,
        ServiceProvider serviceProvider,
        IServiceProvider scopedServices);

    private readonly AddCustomSeedDataHandler? _addCustomSeedDataHandler;
    private readonly bool _addTestAuthentication;

    private readonly SqliteConnection _connection = new("DataSource=:memory:");
    private readonly RegisterCustomServicesHandler? _registerCustomServicesHandler;

    public CleanArchitectureWebApplicationFactory(
        AddCustomSeedDataHandler? addCustomSeedDataHandler,
        RegisterCustomServicesHandler? registerCustomServicesHandler,
        bool addTestAuthentication)
    {
        _addCustomSeedDataHandler = addCustomSeedDataHandler;
        _registerCustomServicesHandler = registerCustomServicesHandler;
        _addTestAuthentication = addTestAuthentication;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Integration");

        base.ConfigureWebHost(builder);

        _connection.Open();

        builder.ConfigureServices(services =>
        {
            services.SetupTestDatabase<ApplicationDbContext>(_connection);
            services.SetupTestDatabase<EventStoreDbContext>(_connection);
            services.SetupTestDatabase<DomainNotificationStoreDbContext>(_connection);

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

            var applicationDbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
            var storeDbContext = scopedServices.GetRequiredService<EventStoreDbContext>();
            var domainStoreDbContext = scopedServices.GetRequiredService<DomainNotificationStoreDbContext>();

            applicationDbContext.EnsureMigrationsApplied();

            var creator2 = (RelationalDatabaseCreator)storeDbContext.Database
                .GetService<IRelationalDatabaseCreator>();
            creator2.CreateTables();

            var creator3 = (RelationalDatabaseCreator)domainStoreDbContext
                .Database.GetService<IRelationalDatabaseCreator>();
            creator3.CreateTables();

            _addCustomSeedDataHandler?.Invoke(applicationDbContext);
            _registerCustomServicesHandler?.Invoke(services, sp, scopedServices);
        });
    }
}