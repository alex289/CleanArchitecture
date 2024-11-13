using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class SendAcknowledgement : IRabbitMqAction
{
    public ulong DeliveryTag { get; }

    public SendAcknowledgement(ulong deliveryTag)
    {
        DeliveryTag = deliveryTag;
    }

    public async Task Perform(IChannel channel)
    {
        await channel.BasicAckAsync(DeliveryTag, false);
    }
}