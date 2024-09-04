using System;
using MediatR;

namespace CleanArchitecture.Shared.Events;

public abstract class DomainEvent : Message, INotification
{
    public DateTime Timestamp { get; private set; }

    protected DomainEvent(Guid aggregateId) : base(aggregateId)
    {
        Timestamp = DateTime.UtcNow;
    }

    protected DomainEvent(Guid aggregateId, string? messageType) : base(aggregateId, messageType)
    {
        Timestamp = DateTime.UtcNow;
    }
}