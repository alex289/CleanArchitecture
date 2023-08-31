using System;
using System.Collections.Generic;
using CleanArchitecture.Application.Queries.Tenants.GetAll;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Tenants;

public sealed class GetAllTenantsTestFixture : QueryHandlerBaseFixture
{
    public GetAllTenantsQueryHandler QueryHandler { get; }
    private ITenantRepository TenantRepository { get; }

    public GetAllTenantsTestFixture()
    {
        TenantRepository = Substitute.For<ITenantRepository>();

        QueryHandler = new GetAllTenantsQueryHandler(TenantRepository);
    }

    public Tenant SetupTenant(bool deleted = false)
    {
        var tenant = new Tenant(Guid.NewGuid(), "Tenant 1");

        if (deleted)
        {
            tenant.Delete();
        }

        var tenantList = new List<Tenant> { tenant }.BuildMock();
        TenantRepository.GetAllNoTracking().Returns(tenantList);

        return tenant;
    }
}