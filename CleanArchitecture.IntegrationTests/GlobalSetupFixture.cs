using System;
using System.Threading.Tasks;
using Respawn;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace CleanArchitecture.IntegrationTests;

[SetUpFixture]
internal class GlobalSetupFixture
{
    private static Respawner? s_respawner;

    public static MsSqlContainer DatabaseContainer { get; } = new MsSqlBuilder()
        // Required for https://github.com/docker/for-mac/issues/7368
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
        .WithPortBinding(MsSqlBuilder.MsSqlPort, assignRandomHostPort: true)
        .Build();

    public static RedisContainer RedisContainer { get; } = new RedisBuilder()
        .WithPortBinding(RedisBuilder.RedisPort, assignRandomHostPort: true)
        .Build();

    public static RabbitMqContainer RabbitContainer { get; } = new RabbitMqBuilder()
        .WithUsername("guest")
        .WithPassword("guest")
        .WithPortBinding(RabbitMqBuilder.RabbitMqPort, assignRandomHostPort: true)
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
            .Replace("Database=master", $"Database=clean-architecture-{Guid.NewGuid()}");
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
                await TestContext.Out.WriteLineAsync($"Failed to create respawner: {ex.Message}");
            }
        }

        if (s_respawner is not null)
        {
            await s_respawner.ResetAsync(DatabaseConnectionString);
        }
    }
}