using System;
using CleanArchitecture.Domain.Commands.Users.CreateUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.CreateUser;

public sealed class CreateUserCommandTestFixture : CommandHandlerFixtureBase
{
    public CreateUserCommandHandler CommandHandler { get; }
    public IUserRepository UserRepository { get; }
    private ITenantRepository TenantRepository { get; }

    public CreateUserCommandTestFixture()
    {
        UserRepository = Substitute.For<IUserRepository>();
        TenantRepository = Substitute.For<ITenantRepository>();

        CommandHandler = new CreateUserCommandHandler(
            Bus,
            UnitOfWork,
            NotificationHandler,
            UserRepository,
            TenantRepository,
            User);
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

    public void SetupCurrentUser()
    {
        var userId = Guid.NewGuid();

        User.GetUserId().Returns(userId);

        UserRepository
            .GetByIdAsync(Arg.Is<Guid>(y => y == userId))
            .Returns(new Entities.User(
                userId,
                Guid.NewGuid(),
                "some email",
                "some first name",
                "some last name",
                "some password",
                UserRole.Admin));
    }

    public void SetupTenant(Guid tenantId)
    {
        TenantRepository
            .ExistsAsync(Arg.Is<Guid>(y => y == tenantId))
            .Returns(true);
    }
}