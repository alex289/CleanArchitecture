using System.Threading.Tasks;
using CleanArchitecture.Shared.Events;

namespace CleanArchitecture.Domain.EventHandler.Fanout;

public interface IFanoutEventHandler
{
    Task<DomainEvent> HandleDomainEventAsync(DomainEvent @event);
}