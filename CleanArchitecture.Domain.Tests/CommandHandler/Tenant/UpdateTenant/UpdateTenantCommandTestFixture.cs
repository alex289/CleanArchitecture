using System;
using CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.UpdateTenant;

public sealed class UpdateTenantCommandTestFixture : CommandHandlerFixtureBase
{
    public UpdateTenantCommandHandler CommandHandler { get; }

    private ITenantRepository TenantRepository { get; }

    public UpdateTenantCommandTestFixture()
    {
        TenantRepository = Substitute.For<ITenantRepository>();

        CommandHandler = new UpdateTenantCommandHandler(
            Bus,
            UnitOfWork,
            NotificationHandler,
            TenantRepository,
            User);
    }

    public void SetupUser()
    {
        User.GetUserRole().Returns(UserRole.User);
    }

    public void SetupExistingTenant(Guid id)
    {
        TenantRepository
            .GetByIdAsync(Arg.Is<Guid>(x => x == id))
            .Returns(new Entities.Tenant(id, "Test Tenant"));
    }
}