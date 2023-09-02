using System;

namespace CleanArchitecture.Shared.Events.Tenant;

public sealed class TenantUpdatedEvent : DomainEvent
{
    public string Name { get; set; }

    public TenantUpdatedEvent(Guid tenantId, string name) : base(tenantId)
    {
        Name = name;
    }
}