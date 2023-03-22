using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class PasswordChangedEvent : DomainEvent
{
    public PasswordChangedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}