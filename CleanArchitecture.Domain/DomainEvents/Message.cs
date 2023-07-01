using System;
using MediatR;

namespace CleanArchitecture.Domain.DomainEvents;

public abstract class Message : IRequest
{
    protected Message(Guid aggregateId)
    {
        AggregateId = aggregateId;
        MessageType = GetType().Name;
    }

    protected Message(Guid aggregateId, string? messageType)
    {
        AggregateId = aggregateId;
        MessageType = messageType ?? string.Empty;
    }

    public Guid AggregateId { get; private set; }
    public string MessageType { get; protected set; }
}
