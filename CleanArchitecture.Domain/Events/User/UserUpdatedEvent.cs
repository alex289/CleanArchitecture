using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserUpdatedEvent : DomainEvent
{
    public Guid TenantId { get; }

    public UserUpdatedEvent(Guid userId, Guid tenantId) : base(userId)
    {
        TenantId = tenantId;
    }
}