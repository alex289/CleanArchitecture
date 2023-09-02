using System;

namespace CleanArchitecture.Shared.Events.User;

public sealed class UserUpdatedEvent : DomainEvent
{
    public Guid TenantId { get; }

    public UserUpdatedEvent(Guid userId, Guid tenantId) : base(userId)
    {
        TenantId = tenantId;
    }
}