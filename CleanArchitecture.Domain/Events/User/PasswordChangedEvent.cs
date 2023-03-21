using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class PasswordChangedEvent : DomainEvent
{
    public Guid UserId { get; }

    public PasswordChangedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }
}
