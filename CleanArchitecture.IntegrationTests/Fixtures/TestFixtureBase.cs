using System;
using System.Net.Http;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.IntegrationTests.Extensions;
using CleanArchitecture.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public class TestFixtureBase
{
    public TestFixtureBase()
    {
        Factory = new CleanArchitectureWebApplicationFactory(
            SeedTestData,
            RegisterCustomServicesHandler);

        ServerClient = Factory.CreateClient();
        ServerClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public HttpClient ServerClient { get; }
    protected WebApplicationFactory<Program> Factory { get; }

    protected virtual void SeedTestData(ApplicationDbContext context)
    {
    }

    protected virtual void RegisterCustomServicesHandler(
        IServiceCollection services,
        ServiceProvider serviceProvider,
        IServiceProvider scopedServices)
    {
    }

    // Todo: Fix auth
    public virtual async Task AuthenticateUserAsync()
    {
        ServerClient.DefaultRequestHeaders.Clear();
        var user = new LoginUserViewModel(
            "admin@email.com",
            "!Password123#");

        var response = await ServerClient.PostAsJsonAsync("/api/v1/user/login", user);

        var message = await response.Content.ReadAsJsonAsync<string>();
        ServerClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {message!.Data}");
    }
}