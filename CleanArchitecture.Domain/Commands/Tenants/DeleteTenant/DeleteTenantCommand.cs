using System;

namespace CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;

public sealed class DeleteTenantCommand : CommandBase
{
    private static readonly DeleteTenantCommandValidation s_validation = new();

    public DeleteTenantCommand(Guid tenantId) : base(tenantId)
    {
    }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}