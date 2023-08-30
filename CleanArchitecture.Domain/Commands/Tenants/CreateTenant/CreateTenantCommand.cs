using System;

namespace CleanArchitecture.Domain.Commands.Tenants.CreateTenant;

public sealed class CreateTenantCommand : CommandBase
{
    private static readonly CreateTenantCommandValidation s_validation = new();

    public CreateTenantCommand(Guid tenantId, string name) : base(tenantId)
    {
        Name = name;
    }

    public string Name { get; }

    public override bool IsValid()
    {
        ValidationResult = s_validation.Validate(this);
        return ValidationResult.IsValid;
    }
}