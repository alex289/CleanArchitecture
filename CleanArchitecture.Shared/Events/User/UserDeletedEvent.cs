using System;

namespace CleanArchitecture.Shared.Events.User;

public sealed class UserDeletedEvent : DomainEvent
{
    public Guid TenantId { get; }

    public UserDeletedEvent(Guid userId, Guid tenantId) : base(userId)
    {
        TenantId = tenantId;
    }
}