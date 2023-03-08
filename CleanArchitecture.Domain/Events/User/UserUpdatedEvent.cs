using System;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserUpdatedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserUpdatedEvent(Guid userId) : base(userId)
    {
        UserId = userId;
    }
}