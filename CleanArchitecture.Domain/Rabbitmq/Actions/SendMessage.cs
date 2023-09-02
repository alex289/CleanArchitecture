using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CleanArchitecture.Domain.Rabbitmq.Actions;

public sealed class SendMessage : IRabbitMqAction
{
    private static readonly JsonSerializerSettings s_serializerSettings =
        new() { TypeNameHandling = TypeNameHandling.Objects };

    private readonly string _exchange;
    private readonly object _message;

    private readonly string _routingKey;


    /// <param name="routingKey">If exchange is empty, this is the name of the queue</param>
    public SendMessage(string routingKey, string exchange, object message)
    {
        _routingKey = routingKey;
        _exchange = exchange;
        _message = message;
    }

    public void Perform(IModel channel)
    {
        var json = JsonConvert.SerializeObject(_message, s_serializerSettings);

        var content = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            _exchange,
            _routingKey,
            null,
            content);
    }
}