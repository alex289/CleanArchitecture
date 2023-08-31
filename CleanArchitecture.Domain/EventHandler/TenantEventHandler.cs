using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Events.Tenant;
using MediatR;

namespace CleanArchitecture.Domain.EventHandler;

public sealed class TenantEventHandler :
    INotificationHandler<TenantCreatedEvent>,
    INotificationHandler<TenantDeletedEvent>,
    INotificationHandler<TenantUpdatedEvent>
{
    public Task Handle(TenantCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(TenantDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(TenantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}