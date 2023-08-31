using System;
using System.Net.Http;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.IntegrationTests.Infrastructure;
using CleanArchitecture.IntegrationTests.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public class TestFixtureBase
{
    public HttpClient ServerClient { get; }
    protected WebApplicationFactory<Program> Factory { get; }

    public TestFixtureBase(bool useTestAuthentication = true)
    {
        Factory = new CleanArchitectureWebApplicationFactory(
            SeedTestData,
            RegisterCustomServicesHandler,
            useTestAuthentication);

        ServerClient = Factory.CreateClient();
        ServerClient.Timeout = TimeSpan.FromMinutes(5);
    }

    protected virtual void SeedTestData(ApplicationDbContext context)
    {
        context.Users.Add(new User(
            TestAuthenticationOptions.TestUserId,
            Ids.Seed.TenantId,
            TestAuthenticationOptions.Email,
            TestAuthenticationOptions.FirstName,
            TestAuthenticationOptions.LastName,
            // !Password123#
            "$2a$12$Blal/uiFIJdYsCLTMUik/egLbfg3XhbnxBC6Sb5IKz2ZYhiU/MzL2",
            UserRole.Admin));

        context.SaveChanges();
    }

    protected virtual void RegisterCustomServicesHandler(
        IServiceCollection services,
        ServiceProvider serviceProvider,
        IServiceProvider scopedServices)
    {
    }
}