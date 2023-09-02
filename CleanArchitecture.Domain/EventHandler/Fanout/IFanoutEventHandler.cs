using System.Threading.Tasks;
using CleanArchitecture.Domain.DomainEvents;

namespace CleanArchitecture.Domain.EventHandler.Fanout;

public interface IFanoutEventHandler
{
    Task<DomainEvent> HandleDomainEventAsync(DomainEvent @event);
}