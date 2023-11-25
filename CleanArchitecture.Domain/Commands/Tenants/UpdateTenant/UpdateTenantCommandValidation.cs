using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Tenants.UpdateTenant;

public sealed class UpdateTenantCommandValidation : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidation()
    {
        AddRuleForId();
        AddRuleForName();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.AggregateId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.EmptyId)
            .WithMessage("Tenant id may not be empty");
    }

    private void AddRuleForName()
    {
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.EmptyName)
            .WithMessage("Name may not be empty")
            .MaximumLength(MaxLengths.Tenant.Name)
            .WithErrorCode(DomainErrorCodes.Tenant.NameExceedsMaxLength)
            .WithMessage($"Name may not be longer than {MaxLengths.Tenant.Name} characters");
    }
}