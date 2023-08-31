using System;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.UpdateUser;

public sealed class UpdateUserCommandTestFixture : CommandHandlerFixtureBase
{
    public UpdateUserCommandHandler CommandHandler { get; }
    public IUserRepository UserRepository { get; }
    private ITenantRepository TenantRepository { get; }

    public UpdateUserCommandTestFixture()
    {
        UserRepository = Substitute.For<IUserRepository>();
        TenantRepository = Substitute.For<ITenantRepository>();

        CommandHandler = new UpdateUserCommandHandler(
            Bus,
            UnitOfWork,
            NotificationHandler,
            UserRepository,
            User,
            TenantRepository);
    }

    public Entities.User SetupUser()
    {
        var user = new Entities.User(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "max@mustermann.com",
            "Max",
            "Mustermann",
            "Password",
            UserRole.User);

        UserRepository
            .GetByIdAsync(Arg.Is<Guid>(y => y == user.Id))
            .Returns(user);

        return user;
    }

    public Entities.Tenant SetupTenant(Guid tenantId)
    {
        var tenant = new Entities.Tenant(tenantId, "Name");

        TenantRepository
            .ExistsAsync(Arg.Is<Guid>(y => y == tenant.Id))
            .Returns(true);

        return tenant;
    }

    public void SetupCurrentUser(Guid userId)
    {
        User.GetUserId().Returns(userId);
        User.GetUserRole().Returns(UserRole.User);
    }
}