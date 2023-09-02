using System.Threading.Tasks;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Rabbitmq;
using CleanArchitecture.Shared.Events;

namespace CleanArchitecture.Domain.EventHandler.Fanout;

public sealed class FanoutEventHandler : IFanoutEventHandler
{
    private readonly RabbitMqHandler _rabbitMqHandler;

    public FanoutEventHandler(
        RabbitMqHandler rabbitMqHandler)
    {
        _rabbitMqHandler = rabbitMqHandler;
        _rabbitMqHandler.InitializeExchange(Messaging.ExchangeNameNotifications);
    }

    public Task<DomainEvent> HandleDomainEventAsync(DomainEvent @event)
    {
        _rabbitMqHandler.EnqueueExchangeMessage(
            Messaging.ExchangeNameNotifications,
            @event);

        return Task.FromResult(@event);
    }
}