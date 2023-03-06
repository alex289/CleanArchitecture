using System;

namespace CleanArchitecture.Domain.Notifications;

public sealed class DomainNotification : DomainEvent
{
    public string Key { get; private set; }
    public string Value { get; private set; }
    public string Code { get; private set; }
    public object? Data { get; set; }

    public DomainNotification(
        string key,
        string value,
        string code,
        object? data = null,
        Guid? aggregateId = null)
        : base(aggregateId ?? Guid.Empty)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Code = code ?? throw new ArgumentNullException(nameof(code));

        Data = data;
    }
}