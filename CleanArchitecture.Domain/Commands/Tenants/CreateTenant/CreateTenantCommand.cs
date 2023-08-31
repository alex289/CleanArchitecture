using System;

namespace CleanArchitecture.Domain.Commands.Tenants.CreateTenant;

public sealed class CreateTenantCommand : CommandBase
{
    private static readonly CreateTenantCommandValidation s_validation = new();

    public string Name { get; }

    public CreateTenantCommand(Guid tenantId, string name) : base(tenantId)
    {
        Name = name;
    }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}