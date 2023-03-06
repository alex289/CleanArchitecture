using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserDeletedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserDeletedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }
}