using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(Guid userId) : base(userId)
    {
    }
}