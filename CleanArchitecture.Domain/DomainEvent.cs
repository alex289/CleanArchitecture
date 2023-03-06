using System;
using MediatR;

namespace CleanArchitecture.Domain;

public abstract class DomainEvent : INotification
{
    protected DomainEvent(Guid aggregateId)
    {
        Timestamp = DateTime.Now;
    }

    private DateTime Timestamp { get; }
}