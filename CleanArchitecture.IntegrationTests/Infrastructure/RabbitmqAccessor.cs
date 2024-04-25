using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Rabbitmq;
using CleanArchitecture.Domain.Rabbitmq.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;
using RabbitMqConfiguration = CleanArchitecture.Domain.Rabbitmq.RabbitMqConfiguration;

namespace CleanArchitecture.IntegrationTests.Infrastructure;

public sealed class RabbitmqAccessor
{
    private static readonly ConcurrentDictionary<string, RabbitmqAccessor> s_accessors = new();

    private static readonly RabbitMqContainer s_rabbitContainer = new RabbitMqBuilder()
        .WithPortBinding(5672)
        .Build();

    public async Task InitializeAsync()
    {
        await s_rabbitContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await s_rabbitContainer.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return s_rabbitContainer.GetConnectionString();
    }

    public void RegisterRabbitmq(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rabbitService = serviceCollection.FirstOrDefault(x =>
            x.ServiceType == typeof(RabbitMqHandler));

        if (rabbitService != null)
        {
            serviceCollection.Remove(rabbitService);
        }

        var rabbitConfig = serviceCollection.FirstOrDefault(x =>
            x.ServiceType == typeof(RabbitMqConfiguration));

        if (rabbitConfig != null)
        {
            serviceCollection.Remove(rabbitConfig);
        }

        serviceCollection.AddRabbitMqHandler(
            configuration,
            "RabbitMQ");
    }

    public static RabbitmqAccessor GetOrCreateAsync()
    {
        return s_accessors.GetOrAdd("rabbimq", _ => new RabbitmqAccessor());
    }
}