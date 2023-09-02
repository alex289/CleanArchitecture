using System;

namespace CleanArchitecture.Shared.Events.Tenant;

public sealed class TenantDeletedEvent : DomainEvent
{
    public TenantDeletedEvent(Guid tenantId) : base(tenantId)
    {
    }
}