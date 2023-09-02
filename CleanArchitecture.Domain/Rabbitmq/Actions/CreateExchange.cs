using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class CreateExchange : IRabbitMqAction
{
    private readonly string _name;
    private readonly string _type;

    public CreateExchange(string name, string type)
    {
        _name = name;
        _type = type;
    }

    public void Perform(IModel channel)
    {
        channel.ExchangeDeclare(_name, _type);
    }
}