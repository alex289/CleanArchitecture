using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class SendAcknowledgement : IRabbitMqAction
{
    public ulong DeliveryTag { get; }

    public SendAcknowledgement(ulong deliveryTag)
    {
        DeliveryTag = deliveryTag;
    }

    public void Perform(IModel channel)
    {
        channel.BasicAck(DeliveryTag, false);
    }
}