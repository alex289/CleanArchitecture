using System;
using CleanArchitecture.Domain.Notifications;

namespace CleanArchitecture.Domain.DomainNotifications;

public class StoredDomainNotification : DomainNotification
{
    public Guid Id { get; private set; }
    public string SerializedData { get; private set; } = string.Empty;
    public string User { get; private set; } = string.Empty;
    public string CorrelationId { get; private set; } = string.Empty;

    public StoredDomainNotification(
        DomainNotification domainNotification,
        string data,
        string user,
        string correlationId) : base(
        domainNotification.Key,
        domainNotification.Value,
        domainNotification.Code,
        null,
        domainNotification.AggregateId)
    {
        Id = Guid.NewGuid();
        User = user;
        SerializedData = data;
        CorrelationId = correlationId;

        MessageType = domainNotification.MessageType;
    }

    // EF Constructor
    protected StoredDomainNotification() : base(string.Empty, string.Empty, string.Empty)
    {
    }
}