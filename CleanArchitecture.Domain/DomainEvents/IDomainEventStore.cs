using System.Threading.Tasks;
using CleanArchitecture.Shared.Events;

namespace CleanArchitecture.Domain.DomainEvents;

public interface IDomainEventStore
{
    Task SaveAsync<T>(T domainEvent) where T : DomainEvent;
}