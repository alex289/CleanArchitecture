using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Shared.Events.User;
using NSubstitute;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.User.UpdateUser;

public sealed class UpdateUserCommandHandlerTests
{
    private readonly UpdateUserCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Update_User()
    {
        var user = _fixture.SetupUser();

        var command = new UpdateUserCommand(
            user.Id,
            "test@email.com",
            "Test",
            "Email",
            UserRole.User,
            Guid.NewGuid());

        _fixture.SetupTenant(command.TenantId);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<UserUpdatedEvent>(x => x.AggregateId == command.UserId);
    }

    [Fact]
    public async Task Should_Not_Update_Non_Existing_User()
    {
        _fixture.SetupUser();

        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "test@email.com",
            "Test",
            "Email",
            UserRole.User,
            Guid.NewGuid());

        _fixture.SetupTenant(command.TenantId);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no user with Id {command.UserId}");
    }

    [Fact]
    public async Task Should_Not_Update_With_Existing_User_Email()
    {
        var user = _fixture.SetupUser();

        var command = new UpdateUserCommand(
            user.Id,
            "test@email.com",
            "Test",
            "Email",
            UserRole.User,
            Guid.NewGuid());

        _fixture.SetupTenant(command.TenantId);

        _fixture.UserRepository
            .GetByEmailAsync(command.Email)
            .Returns(new Entities.User(
                Guid.NewGuid(),
                Guid.NewGuid(),
                command.Email,
                "Some",
                "User",
                "234fs@#*@#",
                UserRole.User));

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                DomainErrorCodes.User.AlreadyExists,
                $"There is already a user with email {command.Email}");
    }

    [Fact]
    public async Task Should_Not_Update_Non_Existing_Tenant()
    {
        var user = _fixture.SetupUser();

        var command = new UpdateUserCommand(
            user.Id,
            "test@email.com",
            "Test",
            "Email",
            UserRole.User,
            Guid.NewGuid());

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<UserUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no tenant with Id {command.TenantId}");
    }

    [Fact]
    public async Task Should_Not_Update_Admin_Properties()
    {
        var user = _fixture.SetupUser();
        _fixture.SetupCurrentUser(user.Id);

        var command = new UpdateUserCommand(
            user.Id,
            "test@email.com",
            "Test",
            "Email",
            UserRole.Admin,
            Guid.NewGuid());

        _fixture.SetupTenant(command.TenantId);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture.UserRepository.Received(1).Update(Arg.Is<Entities.User>(u =>
            u.TenantId == user.TenantId &&
            u.Role == user.Role &&
            u.Id == command.UserId &&
            u.Email == command.Email &&
            u.FirstName == command.FirstName));

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<UserUpdatedEvent>(x => x.AggregateId == command.UserId);
    }
}