using System.Threading.Tasks;
using CleanArchitecture.Shared.Events;

namespace CleanArchitecture.Domain.EventHandler.Fanout;

public interface IFanoutEventHandler
{
    Task<T> HandleDomainEventAsync<T>(T @event) where T : DomainEvent;
}