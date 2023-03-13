using System;
using System.Linq.Expressions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Notifications;
using Moq;

namespace CleanArchitecture.Domain.Tests;

public class CommandHandlerFixtureBase
{
    protected Mock<IMediatorHandler> Bus { get; }
    protected Mock<IUnitOfWork> UnitOfWork { get; }
    protected Mock<DomainNotificationHandler> NotificationHandler { get; }

    protected CommandHandlerFixtureBase()
    {
        Bus = new Mock<IMediatorHandler>();
        UnitOfWork = new Mock<IUnitOfWork>();
        NotificationHandler = new Mock<DomainNotificationHandler>();

        UnitOfWork.Setup(unit => unit.CommitAsync()).ReturnsAsync(true);
    }

    public CommandHandlerFixtureBase VerifyExistingNotification(string errorCode, string message)
    {
        Bus.Verify(
            bus => bus.RaiseEventAsync(
                It.Is<DomainNotification>(not => not.Code == errorCode && not.Value == message)),
            Times.Once);

        return this;
    }

    public CommandHandlerFixtureBase VerifyAnyDomainNotification()
    {
        Bus.Verify(
            bus => bus.RaiseEventAsync(It.IsAny<DomainNotification>()),
            Times.Once);

        return this;
    }

    public CommandHandlerFixtureBase VerifyNoDomainNotification()
    {
        Bus.Verify(
            bus => bus.RaiseEventAsync(It.IsAny<DomainNotification>()),
            Times.Never);

        return this;
    }

    public CommandHandlerFixtureBase VerifyNoRaisedEvent<TEvent>()
        where TEvent : DomainEvent
    {
        Bus.Verify(
            bus => bus.RaiseEventAsync(It.IsAny<TEvent>()),
            Times.Never);

        return this;
    }

    public CommandHandlerFixtureBase VerifyNoRaisedEvent<TEvent>(Expression<Func<TEvent, bool>> checkFunction)
        where TEvent : DomainEvent
    {
        Bus.Verify(bus => bus.RaiseEventAsync(It.Is(checkFunction)), Times.Never);

        return this;
    }

    public CommandHandlerFixtureBase VerifyNoCommit()
    {
        UnitOfWork.Verify(unit => unit.CommitAsync(), Times.Never);

        return this;
    }

    public CommandHandlerFixtureBase VerifyCommit()
    {
        UnitOfWork.Verify(unit => unit.CommitAsync(), Times.Once);

        return this;
    }

    public CommandHandlerFixtureBase VerifyRaisedEvent<TEvent>(Expression<Func<TEvent, bool>> checkFunction)
        where TEvent : DomainEvent
    {
        Bus.Verify(bus => bus.RaiseEventAsync(It.Is(checkFunction)), Times.Once);

        return this;
    }
}