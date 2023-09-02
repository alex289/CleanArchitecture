using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public interface IRabbitMqAction
{
    void Perform(IModel channel);
}