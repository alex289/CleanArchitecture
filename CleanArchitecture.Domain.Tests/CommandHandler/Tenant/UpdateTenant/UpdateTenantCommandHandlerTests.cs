using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Shared.Events.Tenant;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.UpdateTenant;

public sealed class UpdateTenantCommandHandlerTests
{
    private readonly UpdateTenantCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Update_Tenant()
    {
        var command = new UpdateTenantCommand(
            Guid.NewGuid(),
            "Tenant Name");

        _fixture.SetupExistingTenant(command.AggregateId);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyCommit()
            .VerifyNoDomainNotification()
            .VerifyRaisedEvent<TenantUpdatedEvent>(x =>
                x.AggregateId == command.AggregateId &&
                x.Name == command.Name);
    }

    [Fact]
    public async Task Should_Not_Update_Tenant_Insufficient_Permissions()
    {
        var command = new UpdateTenantCommand(
            Guid.NewGuid(),
            "Tenant Name");

        _fixture.SetupUser();

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.InsufficientPermissions,
                $"No permission to update tenant {command.AggregateId}");
    }

    [Fact]
    public async Task Should_Not_Update_Tenant_Not_Existing()
    {
        var command = new UpdateTenantCommand(
            Guid.NewGuid(),
            "Tenant Name");

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no tenant with Id {command.AggregateId}");
    }
}