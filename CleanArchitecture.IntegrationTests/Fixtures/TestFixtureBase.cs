using System;
using System.IO;
using System.Net.Http;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public class TestFixtureBase
{
    public TestFixtureBase()
    {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "appsettings.Integration.json");

        var factory = new CleanArchitectureWebApplicationFactory(
            SeedTestData,
            RegisterCustomServicesHandler,
            configPath);

        ServerClient = factory.CreateClient();
        ServerClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public HttpClient ServerClient { get; }

    protected virtual void SeedTestData(ApplicationDbContext context)
    {
    }

    protected virtual void RegisterCustomServicesHandler(
        IServiceCollection services,
        ServiceProvider serviceProvider,
        IServiceProvider scopedServices)
    {
    }
}