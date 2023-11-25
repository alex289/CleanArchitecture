using System;
using CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;
using CleanArchitecture.Domain.Errors;
using Xunit;

namespace CleanArchitecture.Domain.Tests.CommandHandler.Tenant.UpdateTenant;

public sealed class UpdateTenantCommandValidationTests :
    ValidationTestBase<UpdateTenantCommand, UpdateTenantCommandValidation>
{
    public UpdateTenantCommandValidationTests() : base(new UpdateTenantCommandValidation())
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

    [Fact]
    public void Should_Be_Invalid_For_Empty_Tenant_Name()
    {
        var command = CreateTestCommand(name: "");

        ShouldHaveSingleError(
            command,
            DomainErrorCodes.Tenant.EmptyName,
            "Name may not be empty");
    }

    private static UpdateTenantCommand CreateTestCommand(
        Guid? id = null,
        string? name = null)
    {
        return new UpdateTenantCommand(
            id ?? Guid.NewGuid(),
            name ?? "Test Tenant");
    }
}