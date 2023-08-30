using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Tenants.CreateTenant;

public sealed class CreateTenantCommandValidation : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidation()
    {
        AddRuleForId();
        AddRuleForName();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.AggregateId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.TenantEmptyId)
            .WithMessage("Tenant id may not be empty");
    }

    private void AddRuleForName()
    {
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.TenantEmptyName)
            .WithMessage("Name may not be empty")
            .MaximumLength(MaxLengths.Tenant.Name)
            .WithErrorCode(DomainErrorCodes.Tenant.TenantNameExceedsMaxLength)
            .WithMessage($"Name may not be longer than {MaxLengths.Tenant.Name} characters");
    }
}