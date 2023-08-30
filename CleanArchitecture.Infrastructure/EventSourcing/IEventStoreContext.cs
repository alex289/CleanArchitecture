namespace CleanArchitecture.Infrastructure.EventSourcing;

public interface IEventStoreContext
{
    public string GetUserEmail();
    public string GetCorrelationId();
}