using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class CreateQueue : IRabbitMqAction
{
    public string QueueName { get; }

    public CreateQueue(string queueName)
    {
        QueueName = queueName;
    }

    public void Perform(IModel channel)
    {
        channel.QueueDeclare(
            QueueName,
            false,
            false,
            false,
            null);
    }
}