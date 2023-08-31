using System;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Database;
using Grpc.Net.Client;

namespace CleanArchitecture.IntegrationTests.Fixtures.gRPC;

public sealed class GetUsersByIdsTestFixture : TestFixtureBase
{
    public GrpcChannel GrpcChannel { get; }
    public Guid CreatedUserId { get; } = Guid.NewGuid();

    public GetUsersByIdsTestFixture()
    {
        GrpcChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Factory.Server.CreateHandler()
        });
    }

    protected override void SeedTestData(ApplicationDbContext context)
    {
        base.SeedTestData(context);

        var user = CreateUser();

        context.Users.Add(user);
        context.SaveChanges();
    }

    public User CreateUser()
    {
        return new User(
            CreatedUserId,
            Ids.Seed.TenantId,
            "user@user.de",
            "User First Name",
            "User Last Name",
            "User Password",
            UserRole.User);
    }
}