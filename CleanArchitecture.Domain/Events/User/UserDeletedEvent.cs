using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.User;

public sealed class UserDeletedEvent : DomainEvent
{
    public UserDeletedEvent(Guid userId) : base(userId)
    {
    }
}