using System;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Database;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public sealed class TenantTestFixture : TestFixtureBase
{
    public Guid CreatedTenantId { get; } = Guid.NewGuid();

    protected override void SeedTestData(ApplicationDbContext context)
    {
        base.SeedTestData(context);

        context.Tenants.Add(new Tenant(
            CreatedTenantId,
            "Test Tenant"));

        context.SaveChanges();
    }
}