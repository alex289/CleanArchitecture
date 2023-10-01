using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Shared.Events.Tenant;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.DeleteTenant;

public sealed class DeleteTenantCommandHandlerTests
{
    private readonly DeleteTenantCommandTestFixture _fixture = new();

    [Fact]
    public async Task Should_Delete_Tenant()
    {
        var tenant = _fixture.SetupTenant();

        var command = new DeleteTenantCommand(tenant.Id);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<TenantDeletedEvent>(x => x.AggregateId == tenant.Id);
    }

    [Fact]
    public async Task Should_Not_Delete_Non_Existing_Tenant()
    {
        _fixture.SetupTenant();

        var command = new DeleteTenantCommand(Guid.NewGuid());

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantDeletedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no tenant with Id {command.AggregateId}");
    }

    [Fact]
    public async Task Should_Not_Delete_Tenant_Insufficient_Permissions()
    {
        var tenant = _fixture.SetupTenant();
        _fixture.SetupUser();

        var command = new DeleteTenantCommand(tenant.Id);

        await _fixture.CommandHandler.Handle(command, default);

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantDeletedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.InsufficientPermissions,
                $"No permission to delete tenant {command.AggregateId}");
    }
}