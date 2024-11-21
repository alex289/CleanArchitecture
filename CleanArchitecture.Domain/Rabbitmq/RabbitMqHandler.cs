using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Rabbitmq.Actions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CleanArchitecture.Domain.Rabbitmq;

public sealed class RabbitMqHandler : BackgroundService
{
    private IChannel? _channel;
    private readonly RabbitMqConfiguration _configuration;

    private readonly ConcurrentDictionary<string, List<ConsumeEventHandler>> _consumers = new();

    private readonly ILogger<RabbitMqHandler> _logger;

    private readonly ConcurrentQueue<IRabbitMqAction> _pendingActions = new();

    public RabbitMqHandler(
        RabbitMqConfiguration configuration,
        ILogger<RabbitMqHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Connection will not be established");
            return;
        }

        _logger.LogInformation("Starting RabbitMQ connection");

        var factory = new ConnectionFactory
        {
            AutomaticRecoveryEnabled = true,
            HostName = _configuration.Host,
            Port = _configuration.Port,
            UserName = _configuration.Username,
            Password = _configuration.Password,
        };

        var connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await connection.CreateChannelAsync(null, cancellationToken);

        await base.StartAsync(cancellationToken);
    }


    public void InitializeExchange(string exchangeName, string type = ExchangeType.Fanout)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping the creation of exchange {exchangeName}.",
                exchangeName);
            return;
        }

        _pendingActions.Enqueue(new CreateExchange(exchangeName, type));
    }

    public void InitializeQueues(params string[] queueNames)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping the creation of queues.");
            return;
        }

        foreach (var queue in queueNames)
        {
            _pendingActions.Enqueue(new CreateQueue(queue));
        }
    }

    public void BindQueueToExchange(string queueName, string exchangeName, string routingKey = "")
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping the binding of queue to exchange.");
            return;
        }

        _pendingActions.Enqueue(new BindQueueToExchange(queueName, exchangeName, routingKey));
    }

    public void AddConsumer(string queueName, ConsumeEventHandler consumer)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping the addition of consumer.");
            return;
        }

        // routingKey is set to queueName to mimic rabbitMQ
        _pendingActions.Enqueue(
            new RegisterConsumer(
                string.Empty,
                queueName,
                queueName,
                consumer,
                AddEventConsumer));
    }

    public void AddExchangeConsumer(string exchange, string routingKey, string queue, ConsumeEventHandler consumer)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping the addition of exchange consumer.");
            return;
        }

        _pendingActions.Enqueue(
            new RegisterConsumer(
                exchange,
                queue,
                routingKey,
                consumer,
                AddEventConsumer));
    }

    public void AddExchangeConsumer(string exchange, string queue, ConsumeEventHandler consumer)
    {
        AddExchangeConsumer(exchange, string.Empty, queue, consumer);
    }

    private async Task AddEventConsumer(string exchange, string queueName, string routingKey, ConsumeEventHandler consumer)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Event consumer will not be added.");
            return;
        }
        
        var key = $"{exchange}-{routingKey}";

        if (!_consumers.TryGetValue(key, out var consumers))
        {
            consumers = new List<ConsumeEventHandler>();
            _consumers.TryAdd(key, consumers);

            var eventHandler = new AsyncEventingBasicConsumer(_channel!);
            eventHandler.ReceivedAsync += CallEventConsumersAsync;

            await _channel!.BasicConsumeAsync(queueName, false, eventHandler);
        }

        consumers.Add(consumer);
    }

    private async Task CallEventConsumersAsync(object sender, BasicDeliverEventArgs ea)
    {
        var key = $"{ea.Exchange}-{ea.RoutingKey}";

        if (!_consumers.TryGetValue(key, out var consumers))
        {
            return;
        }

        foreach (var consumer in consumers)
        {
            try
            {
                await consumer(ea.Body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling event in queue {RoutingKey}", ea.RoutingKey);
            }
        }

        _pendingActions.Enqueue(new SendAcknowledgement(ea.DeliveryTag));
    }


    public void EnqueueMessage(string queueName, object message)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping enqueueing of message");
            return;
        }

        _pendingActions.Enqueue(new SendMessage(queueName, string.Empty, message));
    }

    public void EnqueueExchangeMessage(string exchange, object message, string routingKey = "")
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping enqueueing of message");
            return;
        }

        _pendingActions.Enqueue(new SendMessage(routingKey, exchange, message));
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_configuration.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Message handling loop will not be started");
            return;
        }

        while (true)
        {
            await HandleEnqueuedActions();

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task HandleEnqueuedActions()
    {
        while (_pendingActions.TryDequeue(out var action))
        {
            try
            {
                await action.Perform(_channel!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to send a rabbitmq message");
                _pendingActions.Enqueue(action);
            }
        }
    }
}