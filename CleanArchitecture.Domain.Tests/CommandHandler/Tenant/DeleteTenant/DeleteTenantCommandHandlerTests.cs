using System;
using CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Events.Tenant;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.DeleteTenant;

public sealed class DeleteTenantCommandHandlerTests
{
    private readonly DeleteTenantCommandTestFixture _fixture = new();

    [Fact]
    public void Should_Delete_Tenant()
    {
        var tenant = _fixture.SetupTenant();

        var command = new DeleteTenantCommand(tenant.Id);
        
        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyNoDomainNotification()
            .VerifyCommit()
            .VerifyRaisedEvent<TenantDeletedEvent>(x => x.AggregateId == tenant.Id);
    }
    
    [Fact]
    public void Should_Not_Delete_Non_Existing_Tenant()
    {
        _fixture.SetupTenant();

        var command = new DeleteTenantCommand(Guid.NewGuid());
        
        _fixture.CommandHandler.Handle(command, default).Wait();

        _fixture
            .VerifyNoCommit()
            .VerifyNoRaisedEvent<TenantDeletedEvent>()
            .VerifyAnyDomainNotification()
            .VerifyExistingNotification(
                ErrorCodes.ObjectNotFound,
                $"There is no tenant with Id {command.AggregateId}");
    }
}