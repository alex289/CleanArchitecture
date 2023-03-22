using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}