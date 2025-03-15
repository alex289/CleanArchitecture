using System;
using MassTransit;
using MediatR;

namespace CleanArchitecture.Shared.Events;

[ExcludeFromTopology]
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