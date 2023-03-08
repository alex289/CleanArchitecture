using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserCreatedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserCreatedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }
}