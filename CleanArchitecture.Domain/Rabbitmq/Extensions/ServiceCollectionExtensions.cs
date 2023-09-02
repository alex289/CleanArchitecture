using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Domain.Rabbitmq.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqHandler(
        this IServiceCollection services,
        IConfiguration configuration,
        string rabbitMqConfigSection)
    {
        var rabbitMq = new RabbitMqConfiguration();
        configuration.Bind(rabbitMqConfigSection, rabbitMq);
        services.AddSingleton(rabbitMq);

        services.AddSingleton<RabbitMqHandler>();
        services.AddHostedService(serviceProvider => serviceProvider.GetService<RabbitMqHandler>()!);

        return services;
    }
}