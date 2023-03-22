using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserUpdatedEvent : DomainEvent
{
    public UserUpdatedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}