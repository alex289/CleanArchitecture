using System.Threading.Tasks;
using CleanArchitecture.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Domain.Consumers;

public sealed class FanoutEventConsumer : IConsumer<FanoutDomainEvent>
{
    private readonly ILogger<FanoutEventConsumer> _logger;

    public FanoutEventConsumer(ILogger<FanoutEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<FanoutDomainEvent> context)
    {
        _logger.LogInformation("FanoutDomainEventConsumer: {FanoutDomainEvent}", context.Message);
        return Task.CompletedTask;
    }
}