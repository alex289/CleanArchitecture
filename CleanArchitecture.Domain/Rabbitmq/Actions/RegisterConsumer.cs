using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class RegisterConsumer : IRabbitMqAction
{
    private readonly Func<string, string, string, ConsumeEventHandler, Task> _addConsumer;
    private readonly ConsumeEventHandler _consumer;
    private readonly string _exchange;
    private readonly string _queue;
    private readonly string _routingKey;

    public RegisterConsumer(
        string exchange,
        string queue,
        string routingKey,
        ConsumeEventHandler consumer,
        Func<string, string, string, ConsumeEventHandler, Task> addConsumer)
    {
        _exchange = exchange;
        _queue = queue;
        _routingKey = routingKey;
        _consumer = consumer;
        _addConsumer = addConsumer;
    }

    public async Task Perform(IChannel channel)
    {
        await _addConsumer(_exchange, _queue, _routingKey, _consumer);
    }
}