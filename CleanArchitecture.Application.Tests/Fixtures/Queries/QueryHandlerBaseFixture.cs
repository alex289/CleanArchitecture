using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Notifications;
using Moq;

namespace CleanArchitecture.Application.Tests.Fixtures.Queries;

public class QueryHandlerBaseFixture
{
    public Mock<IMediatorHandler> Bus { get; } = new();
    
    public QueryHandlerBaseFixture VerifyExistingNotification(string key, string errorCode, string message)
    {
        Bus.Verify(
            bus => bus.RaiseEventAsync(
                It.Is<DomainNotification>(
                    notification =>
                        notification.Key == key &&
                        notification.Code == errorCode &&
                        notification.Value == message)),
            Times.Once);

        return this;
    }

    public QueryHandlerBaseFixture VerifyNoDomainNotification()
    {
        Bus.Verify(
            bus => bus.RaiseEventAsync(It.IsAny<DomainNotification>()),
            Times.Never);

        return this;
    }
}