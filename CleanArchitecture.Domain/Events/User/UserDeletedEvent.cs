using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserDeletedEvent : DomainEvent
{
    public Guid TenantId { get; }

    public UserDeletedEvent(Guid userId, Guid tenantId) : base(userId)
    {
        TenantId = tenantId;
    }
}