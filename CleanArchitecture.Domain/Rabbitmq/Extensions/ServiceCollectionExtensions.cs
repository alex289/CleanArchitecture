using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Domain.Rabbitmq.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqHandler(
        this IServiceCollection services,
        RabbitMqConfiguration configuration)
    {
        services.AddSingleton(configuration);

        services.AddSingleton<RabbitMqHandler>();
        services.AddHostedService(serviceProvider => serviceProvider.GetService<RabbitMqHandler>()!);

        return services;
    }
}