using System;
using CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;
using CleanArchitecture.Domain.Errors;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.DeleteTenant;

public sealed class DeleteTenantCommandValidationTests :
    ValidationTestBase<DeleteTenantCommand, DeleteTenantCommandValidation>
{
    public DeleteTenantCommandValidationTests() : base(new DeleteTenantCommandValidation())
    {
    }

    [Fact]
    public void Should_Be_Valid()
    {
        var command = CreateTestCommand();

        ShouldBeValid(command);
    }

    [Fact]
    public void Should_Be_Invalid_For_Empty_Tenant_Id()
    {
        var command = CreateTestCommand(Guid.Empty);

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.Tenant.EmptyId,
            "Tenant id may not be empty");
    }

    private static DeleteTenantCommand CreateTestCommand(Guid? tenantId = null)
    {
        return new DeleteTenantCommand(tenantId ?? Guid.NewGuid());
    }
}