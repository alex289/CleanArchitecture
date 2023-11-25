using CleanArchitecture.Domain.Errors;
using FluentValidation;

namespace CleanArchitecture.Domain.Commands.Tenants.DeleteTenant;

public sealed class DeleteTenantCommandValidation : AbstractValidator<DeleteTenantCommand>
{
    public DeleteTenantCommandValidation()
    {
        AddRuleForId();
    }

    private void AddRuleForId()
    {
        RuleFor(cmd => cmd.AggregateId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Tenant.EmptyId)
            .WithMessage("Tenant id may not be empty");
    }
}