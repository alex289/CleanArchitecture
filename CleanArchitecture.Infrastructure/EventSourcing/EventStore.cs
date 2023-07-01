using CleanArchitecture.Domain.DomainEvents;
using CleanArchitecture.Domain.DomainNotifications;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Infrastructure.Database;
using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure.EventSourcing;

public class DomainEventStore : IDomainEventStore
{
    private readonly EventStoreDbContext _eventStoreDbContext;
    private readonly DomainNotificationStoreDbContext _domainNotificationStoreDbContext;
    private readonly IEventStoreContext _context;

    public DomainEventStore(
        EventStoreDbContext eventStoreDbContext,
        DomainNotificationStoreDbContext domainNotificationStoreDbContext,
        IEventStoreContext context)
    {
        _eventStoreDbContext = eventStoreDbContext;
        _domainNotificationStoreDbContext = domainNotificationStoreDbContext;
        _context = context;
    }

    public async Task SaveAsync<T>(T domainEvent) where T : DomainEvent
    {
        var serializedData = JsonConvert.SerializeObject(domainEvent);

        switch (domainEvent)
        {
            case DomainNotification d:
                var storedDomainNotification = new StoredDomainNotification(
                    d,
                    serializedData,
                    _context.GetUserEmail(),
                    _context.GetCorrelationId());

                _domainNotificationStoreDbContext.StoredDomainNotifications.Add(storedDomainNotification);
                await _domainNotificationStoreDbContext.SaveChangesAsync();

                break;
            default:
                var storedDomainEvent = new StoredDomainEvent(
                    domainEvent,
                    serializedData,
                    _context.GetUserEmail(),
                    _context.GetCorrelationId());

                _eventStoreDbContext.StoredDomainEvents.Add(storedDomainEvent);
                await _eventStoreDbContext.SaveChangesAsync();

                break;
        }
    }
}
