using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserUpdatedEvent : DomainEvent
{
    public UserUpdatedEvent(Guid userId) : base(userId)
    {
    }
}