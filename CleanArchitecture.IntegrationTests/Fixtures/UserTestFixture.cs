using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.IntegrationTests.Infrastructure.Auth;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public sealed class UserTestFixture : TestFixtureBase
{
    public async Task SeedTestData()
    {
        await GlobalSetupFixture.RespawnDatabaseAsync();

        using var context = Factory.Services.GetRequiredService<ApplicationDbContext>();

        context.Users.Add(new User(
            TestAuthenticationOptions.TestUserId,
            Ids.Seed.TenantId,
            TestAuthenticationOptions.Email,
            TestAuthenticationOptions.FirstName,
            TestAuthenticationOptions.LastName,
            TestAuthenticationOptions.Password,
            UserRole.Admin));

        await context.SaveChangesAsync();
    }
}