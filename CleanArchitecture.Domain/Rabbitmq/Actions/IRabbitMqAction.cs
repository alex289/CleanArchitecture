using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public interface IRabbitMqAction
{
    Task Perform(IChannel channel);
}