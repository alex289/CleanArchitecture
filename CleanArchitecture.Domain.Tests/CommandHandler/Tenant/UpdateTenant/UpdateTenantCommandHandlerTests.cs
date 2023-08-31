using System;
using CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.Tenant;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.UpdateTenant;

public sealed class UpdateTenantCommandHandlerTests
{
    private readonly UpdateTenantCommandTestFixture _fixture = new();

    [Fact]
    public void Should_Update_Tenant()
    {
        var command = new UpdateTenantCommand(
            Guid.NewGuid(),
            "Tenant Name");

        _fixture.SetupExistingTenant(command.AggregateId);

        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyCommit()
            .VerifyNoDomainNotification()
            .VerifyRaisedEvent<TenantUpdatedEvent>(x =>
                x.AggregateId == command.AggregateId &&
                x.Name == command.Name);
    }

    [Fact]
    public void Should_Not_Update_Tenant_Insufficient_Permissions()
    {
        var command = new UpdateTenantCommand(
            Guid.NewGuid(),
            "Tenant Name");

        _fixture.SetupUser();

        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.InsufficientPermissions,
                $"No permission to update tenant {command.AggregateId}");
    }

    [Fact]
    public void Should_Not_Update_Tenant_Not_Existing()
    {
        var command = new UpdateTenantCommand(
            Guid.NewGuid(),
            "Tenant Name");

        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantUpdatedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no tenant with Id {command.AggregateId}");
    }
}