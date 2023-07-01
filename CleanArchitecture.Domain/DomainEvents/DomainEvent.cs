using System;
using MediatR;

namespace CleanArchitecture.Domain.DomainEvents;

public abstract class DomainEvent : Message, INotification
{
    protected DomainEvent(Guid aggregateId) : base(aggregateId)
    {
        Timestamp = DateTime.Now;
    }

    protected DomainEvent(Guid aggregateId, string? messageType) : base(aggregateId, messageType)
    {
        Timestamp = DateTime.Now;
    }

    public DateTime Timestamp { get; private set; }
}