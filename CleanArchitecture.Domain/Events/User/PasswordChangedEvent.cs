using System;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.Events.User;

public sealed class PasswordChangedEvent : DomainEvent
{
    public PasswordChangedEvent(Guid userId) : base(userId)
    {
    }
}