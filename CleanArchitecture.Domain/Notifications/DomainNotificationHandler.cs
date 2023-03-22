using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CleanArchitecture.Domain.Notifications;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>
{
    private readonly List<DomainNotification> _notifications;

    public DomainNotificationHandler()
    {
        _notifications = new List<DomainNotification>();
    }

    public Task Handle(DomainNotification notification, CancellationToken cancellationToken = default)
    {
        _notifications.Add(notification);

        return Task.CompletedTask;
    }

    public virtual List<DomainNotification> GetNotifications()
    {
        return _notifications;
    }

    public virtual bool HasNotifications()
    {
        return GetNotifications().Any();
    }

    public virtual void Clear()
    {
        _notifications.Clear();
    }
}