using System;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Database;
using Grpc.Net.Client;

namespace CleanArchitecture.IntegrationTests.Fixtures.gRPC;

public sealed class GetTenantsByIdsTestFixture : TestFixtureBase
{
    public GrpcChannel GrpcChannel { get; }
    public Guid CreatedTenantId { get; } = Guid.NewGuid();

    public GetTenantsByIdsTestFixture()
    {
        GrpcChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Factory.Server.CreateHandler()
        });
    }

    protected override void SeedTestData(ApplicationDbContext context)
    {
        base.SeedTestData(context);

        var tenant = CreateTenant();

        context.Tenants.Add(tenant);
        context.SaveChanges();
    }

    public Tenant CreateTenant()
    {
        return new Tenant(
            CreatedTenantId,
            "Test Tenant");
    }
}