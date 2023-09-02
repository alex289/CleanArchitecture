using System;
using CleanArchitecture.Shared.Events;

namespace CleanArchitecture.Domain.DomainEvents;

public class StoredDomainEvent : DomainEvent
{
    public Guid Id { get; private set; }
    public string Data { get; private set; } = string.Empty;
    public string User { get; private set; } = string.Empty;
    public string CorrelationId { get; private set; } = string.Empty;

    public StoredDomainEvent(
        DomainEvent domainEvent,
        string data,
        string user,
        string correlationId)
        : base(domainEvent.AggregateId, domainEvent.MessageType)
    {
        Id = Guid.NewGuid();
        Data = data;
        User = user;
        CorrelationId = correlationId;
    }

    // EF Constructor
    protected StoredDomainEvent() : base(Guid.NewGuid())
    {
    }
}