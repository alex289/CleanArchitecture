using System;
using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class RegisterConsumer : IRabbitMqAction
{
    private readonly Action<string, string, string, ConsumeEventHandler> _addConsumer;
    private readonly ConsumeEventHandler _consumer;
    private readonly string _exchange;
    private readonly string _queue;
    private readonly string _routingKey;

    public RegisterConsumer(
        string exchange,
        string queue,
        string routingKey,
        ConsumeEventHandler consumer,
        Action<string, string, string, ConsumeEventHandler> addConsumer)
    {
        _exchange = exchange;
        _queue = queue;
        _routingKey = routingKey;
        _consumer = consumer;
        _addConsumer = addConsumer;
    }

    public void Perform(IModel channel)
    {
        _addConsumer(_exchange, _queue, _routingKey, _consumer);
    }
}