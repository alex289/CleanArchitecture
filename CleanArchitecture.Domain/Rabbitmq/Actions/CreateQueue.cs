using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class CreateQueue : IRabbitMqAction
{
    public string QueueName { get; }

    public CreateQueue(string queueName)
    {
        QueueName = queueName;
    }

    public async Task Perform(IChannel channel)
    {
        await channel.QueueDeclareAsync(
            QueueName,
            false,
            false,
            false,
            null);
    }
}