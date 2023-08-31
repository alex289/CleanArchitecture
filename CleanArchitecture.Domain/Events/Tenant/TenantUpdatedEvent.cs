using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.Tenant;

public sealed class TenantUpdatedEvent : DomainEvent
{
    public string Name { get; set; }

    public TenantUpdatedEvent(Guid tenantId, string name) : base(tenantId)
    {
        Name = name;
    }
}