using System;
using System.Threading.Tasks;
using CleanArchitecture.IntegrationTests.Constants;
using Respawn;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace CleanArchitecture.IntegrationTests;

[SetUpFixture]
internal class GlobalSetupFixture
{
    private static Respawner? s_respawner;

    public static PostgreSqlContainer DatabaseContainer { get; } = new PostgreSqlBuilder()
        .WithPortBinding(Configuration.PostgresSqlPort, assignRandomHostPort: true)
        .Build();

    public static RedisContainer RedisContainer { get; } = new RedisBuilder()
        .WithPortBinding(Configuration.RedisPort, assignRandomHostPort: true)
        .Build();

    public static RabbitMqContainer RabbitContainer { get; } = new RabbitMqBuilder()
        .WithUsername("guest")
        .WithPassword("guest")
        .WithPortBinding(Configuration.RabbitMqPort, assignRandomHostPort: true)
        .Build();

    public static string DatabaseConnectionString { get; private set; } = string.Empty;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        await DatabaseContainer.StartAsync();
        await RedisContainer.StartAsync();
        await RabbitContainer.StartAsync();

        DatabaseConnectionString = DatabaseContainer
            .GetConnectionString()
            .Replace("Database=postgres", $"Database=clean-architecture-{Guid.NewGuid()}");
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await DatabaseContainer.DisposeAsync();
        await RedisContainer.DisposeAsync();
        await RabbitContainer.DisposeAsync();
    }

    public static async Task RespawnDatabaseAsync()
    {
        if (s_respawner is null)
        {
            try
            {
                s_respawner = await Respawner.CreateAsync(
                    DatabaseConnectionString,
                    new RespawnerOptions
                    {
                        TablesToIgnore = ["__EFMigrationsHistory"]
                    });
            }
            catch (Exception ex)
            {
                // Creation of the respawner can fail if the database has not been created yet
                TestContext.WriteLine($"Failed to create respawner: {ex.Message}");
            }
        }

        if (s_respawner is not null)
        {
            await s_respawner.ResetAsync(DatabaseConnectionString);
        }
    }
}
