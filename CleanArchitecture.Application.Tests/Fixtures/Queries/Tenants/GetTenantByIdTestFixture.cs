using System;
using CleanArchitecture.Application.Queries.Tenants.GetTenantById;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries.Tenants;

public sealed class GetTenantByIdTestFixture : QueryHandlerBaseFixture
{
    public GetTenantByIdQueryHandler QueryHandler { get; }
    private ITenantRepository TenantRepository { get; }

    public GetTenantByIdTestFixture()
    {
        TenantRepository = Substitute.For<ITenantRepository>();

        QueryHandler = new GetTenantByIdQueryHandler(
            TenantRepository,
            Bus);
    }

    public Tenant SetupTenant(bool deleted = false)
    {
        var tenant = new Tenant(Guid.NewGuid(), "Tenant 1");

        if (deleted)
        {
            tenant.Delete();
        }
        else
        {
            TenantRepository.GetByIdAsync(Arg.Is<Guid>(y => y == tenant.Id)).Returns(tenant);
        }


        return tenant;
    }
}