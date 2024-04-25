using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.MsSql;

namespace CleanArchitecture.IntegrationTests.Infrastructure;

public sealed class DatabaseAccessor
{
    private static readonly ConcurrentDictionary<string, DatabaseAccessor> s_accessors = new();

    private readonly string _instanceDatabaseName;
    private bool _databaseCreated = false;
    private readonly object _databaseCreationLock = new();

    private const string _dbPassword = "234#AD224fD#ss";
    private static readonly MsSqlContainer s_dbContainer = new MsSqlBuilder()
        .WithPassword(_dbPassword)
        .WithPortBinding(1433)
        .Build();

    public DatabaseAccessor(string instanceName)
    {
        _instanceDatabaseName = instanceName;
    }

    public async Task InitializeAsync()
    {
        await s_dbContainer.StartAsync();
    }

    public ApplicationDbContext CreateDatabase(IServiceProvider scopedServices)
    {
        var applicationDbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

        lock (_databaseCreationLock)
        {
            if (_databaseCreated)
            {
                return applicationDbContext;
            }

            applicationDbContext.EnsureMigrationsApplied();

            var eventsContext = scopedServices.GetRequiredService<EventStoreDbContext>();
            eventsContext.EnsureMigrationsApplied();

            var notificationsContext = scopedServices.GetRequiredService<DomainNotificationStoreDbContext>();
            notificationsContext.EnsureMigrationsApplied();
        }

        _databaseCreated = true;

        return applicationDbContext;
    }

    public async ValueTask DisposeAsync()
    {
        // Reset the database to its original state
        var dropScript = $@"
            USE MASTER;

            ALTER DATABASE [{_instanceDatabaseName}]
            SET multi_user WITH ROLLBACK IMMEDIATE;

            ALTER DATABASE [{_instanceDatabaseName}]
            SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

            DROP DATABASE [{_instanceDatabaseName}];";

        await using (var con = new SqlConnection(GetConnectionString()))
        {
            await con.OpenAsync();

            var cmd = new SqlCommand(dropScript, con);
            await cmd.ExecuteNonQueryAsync();
        }

        await s_dbContainer.DisposeAsync();
    }

    public async Task RespawnDatabaseAsync()
    {
        var connectionString = GetConnectionString();

        var respawn = await Respawner.CreateAsync(
            connectionString,
            new RespawnerOptions
            {
                TablesToIgnore = ["__EFMigrationsHistory"]
            });

        await respawn.ResetAsync(connectionString);
    }

    public string GetConnectionString()
    {
        var conBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = s_dbContainer.Hostname,
            InitialCatalog = _instanceDatabaseName,
            IntegratedSecurity = false,
            Password = _dbPassword,
            UserID = "sa",
            TrustServerCertificate = true
        };

        return conBuilder.ToString();
    }

    public static DatabaseAccessor GetOrCreateAsync(string instanceName)
    {
        if (!s_accessors.TryGetValue(instanceName, out _))
        {
            var accessor = new DatabaseAccessor(instanceName);
            s_accessors.TryAdd(instanceName, accessor);
        }

        return s_accessors[instanceName];
    }
}