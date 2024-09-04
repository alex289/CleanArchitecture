using System;
using System.Net.Http;
using CleanArchitecture.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public class TestFixtureBase
{
    public HttpClient ServerClient { get; }
    protected CleanArchitectureWebApplicationFactory Factory { get; }

    public TestFixtureBase(bool useTestAuthentication = true)
    {
        Factory = new CleanArchitectureWebApplicationFactory(
            RegisterCustomServicesHandler,
            useTestAuthentication);

        ServerClient = Factory.CreateClient();
        ServerClient.Timeout = TimeSpan.FromMinutes(5);
    }

    protected virtual void RegisterCustomServicesHandler(
        IServiceCollection services)
    {
    }
}