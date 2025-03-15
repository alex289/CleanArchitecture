using System;

namespace CleanArchitecture.Shared.Events;

public class FanoutDomainEvent : DomainEvent
{
    public DomainEvent DomainEvent { get; }
    public Guid? UserId { get; }
    
    public FanoutDomainEvent(
        Guid aggregateId,
        DomainEvent domainEvent,
        Guid? userId) : base(aggregateId)
    {
        DomainEvent = domainEvent;
        UserId = userId;
    }
}
