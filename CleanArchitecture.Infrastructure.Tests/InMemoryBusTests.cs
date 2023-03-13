using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Events.User;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using Moq;
using Xunit;

namespace CleanArchitecture.Infrastructure.Tests;

public sealed class InMemoryBusTests
{
    [Fact]
    public async Task InMemoryBus_Should_Publish_When_Not_DomainNotification()
    {
        var mediator = new Mock<IMediator>();

        var inMemoryBus = new InMemoryBus(mediator.Object);

        var key = "Key";
        var value = "Value";
        var code = "Code";

        var domainEvent = new DomainNotification(key, value, code);

        await inMemoryBus.RaiseEventAsync(domainEvent);

        mediator.Verify(x => x.Publish(domainEvent, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task InMemoryBus_Should_Save_And_Publish_When_DomainNotification()
    {
        var mediator = new Mock<IMediator>();

        var inMemoryBus = new InMemoryBus(mediator.Object);

        var userDeletedEvent = new UserDeletedEvent(Guid.NewGuid());

        await inMemoryBus.RaiseEventAsync(userDeletedEvent);

        mediator.Verify(x => x.Publish(userDeletedEvent, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task InMemoryBus_Should_Send_Command_Async()
    {
        var mediator = new Mock<IMediator>();

        var inMemoryBus = new InMemoryBus(mediator.Object);

        var deleteUserCommand = new DeleteUserCommand(Guid.NewGuid());

        await inMemoryBus.SendCommandAsync(deleteUserCommand);

        mediator.Verify(x => x.Send(deleteUserCommand, CancellationToken.None), Times.Once);
    }
}