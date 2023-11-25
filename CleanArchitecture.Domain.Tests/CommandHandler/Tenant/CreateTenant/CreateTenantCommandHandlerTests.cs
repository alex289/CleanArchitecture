using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Tenants.CreateTenant;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Shared.Events.Tenant;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.CreateTenant;

public sealed class CreateTenantCommandHandlerTests
{
    private readonly CreateTenantCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Create_Tenant()
    {
        var command = new CreateTenantCommand(
            Guid.NewGuid(),
            "Test Tenant");

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<TenantCreatedEvent>(x =>
                x.AggregateId == command.AggregateId &&
                x.Name == command.Name);
    }

    [Fact]
    public async Task Should_Not_Create_Tenant_Insufficient_Permissions()
    {
        _fixture.SetupUser();

        var command = new CreateTenantCommand(
            Guid.NewGuid(),
            "Test Tenant");

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantCreatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.InsufficientPermissions,
                $"No permission to create tenant {command.AggregateId}");
    }

    [Fact]
    public async Task Should_Not_Create_Tenant_Already_Exists()
    {
        var command = new CreateTenantCommand(
            Guid.NewGuid(),
            "Test Tenant");

        _fixture.SetupExistingTenant(command.AggregateId);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantCreatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                DomainErrorCodes.Tenant.AlreadyExists,
                $"There is already a tenant with Id {command.AggregateId}");
    }
}