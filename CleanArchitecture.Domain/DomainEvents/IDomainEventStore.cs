using System.Threading.Tasks;

namespace CleanArchitecture.Domain.DomainEvents;

public interface IDomainEventStore
{
    Task SaveAsync<T>(T domainEvent) where T : DomainEvent;
}