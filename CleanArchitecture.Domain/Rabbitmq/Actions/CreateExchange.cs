using System.Threading.Tasks;
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

    public async Task Perform(IChannel channel)
    {
        await channel.ExchangeDeclareAsync(_name, _type);
    }
}