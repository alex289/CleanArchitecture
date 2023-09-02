using System;

namespace CleanArchitecture.Shared.Events.User;

public sealed class UserCreatedEvent : DomainEvent
{
    public Guid TenantId { get; }

    public UserCreatedEvent(Guid userId, Guid tenantId) : base(userId)
    {
        TenantId = tenantId;
    }
}