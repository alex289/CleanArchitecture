using System;
using System.Collections.Generic;
using CleanArchitecture.Application.gRPC;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable;
using NSubstitute;

namespace CleanArchitecture.gRPC.Tests.Fixtures;

public sealed class TenantTestFixture
{
    public TenantsApiImplementation TenantsApiImplementation { get; }
    private ITenantRepository TenantRepository { get; }

    public IEnumerable<Tenant> ExistingTenants { get; }

    public TenantTestFixture()
    {
        TenantRepository = Substitute.For<ITenantRepository>();

        ExistingTenants = new List<Tenant>
        {
            new(Guid.NewGuid(), "Tenant 1"),
            new(Guid.NewGuid(), "Tenant 2"),
            new(Guid.NewGuid(), "Tenant 3")
        };

        TenantRepository.GetAllNoTracking().Returns(ExistingTenants.BuildMock());

        TenantsApiImplementation = new TenantsApiImplementation(TenantRepository);
    }
}