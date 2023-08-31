using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.Tenant;

public sealed class TenantDeletedEvent : DomainEvent
{
    public TenantDeletedEvent(Guid tenantId) : base(tenantId)
    {
    }
}