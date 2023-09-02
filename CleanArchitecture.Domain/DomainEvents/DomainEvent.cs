using System;
using MediatR;

namespace CleanArchitecture.Domain.DomainEvents;

// Todo: Move this and all events to shared
public abstract class DomainEvent : Message, INotification
{
    public DateTime Timestamp { get; private set; }

    protected DomainEvent(Guid aggregateId) : base(aggregateId)
    {
        Timestamp = DateTime.Now;
    }

    protected DomainEvent(Guid aggregateId, string? messageType) : base(aggregateId, messageType)
    {
        Timestamp = DateTime.Now;
    }
}