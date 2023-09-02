using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Shared.Events.Tenant;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CleanArchitecture.Domain.EventHandler;

public sealed class TenantEventHandler :
    INotificationHandler<TenantCreatedEvent>,
    INotificationHandler<TenantDeletedEvent>,
    INotificationHandler<TenantUpdatedEvent>
{
    private readonly IDistributedCache _distributedCache;

    public TenantEventHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public Task Handle(TenantCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task Handle(TenantDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(
            CacheKeyGenerator.GetEntityCacheKey<Tenant>(notification.AggregateId),
            cancellationToken);
    }

    public async Task Handle(TenantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(
            CacheKeyGenerator.GetEntityCacheKey<Tenant>(notification.AggregateId),
            cancellationToken);
    }
}