using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.Tenant;

public sealed class TenantUpdatedEvent : DomainEvent
{
    public TenantUpdatedEvent(Guid tenantId, string name) : base(tenantId)
    {
        Name = name;
    }

    public string Name { get; set; }
}