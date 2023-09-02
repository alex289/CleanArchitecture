using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.DomainEvents;
using CleanArchitecture.Domain.EventHandler.Fanout;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Shared.Events.User;
using MediatR;
using NSubstitute;
using Xunit;

namespace CleanArchitecture.Infrastructure.Tests;

public sealed class InMemoryBusTests
{
    [Fact]
    public async Task InMemoryBus_Should_Publish_When_Not_DomainNotification()
    {
        var mediator = Substitute.For<IMediator>();
        var domainEventStore = Substitute.For<IDomainEventStore>();
        var fanoutEventHandler = Substitute.For<IFanoutEventHandler>();

        var inMemoryBus = new InMemoryBus(mediator, domainEventStore, fanoutEventHandler);

        const string key = "Key";
        const string value = "Value";
        const string code = "Code";

        var domainEvent = new DomainNotification(key, value, code);

        await inMemoryBus.RaiseEventAsync(domainEvent);

        await mediator.Received(1).Publish(Arg.Is<DomainNotification>(x => x.Equals(domainEvent)));
    }

    [Fact]
    public async Task InMemoryBus_Should_Save_And_Publish_When_DomainNotification()
    {
        var mediator = Substitute.For<IMediator>();
        var domainEventStore = Substitute.For<IDomainEventStore>();
        var fanoutEventHandler = Substitute.For<IFanoutEventHandler>();

        var inMemoryBus = new InMemoryBus(mediator, domainEventStore, fanoutEventHandler);

        var userDeletedEvent = new UserDeletedEvent(Guid.NewGuid(), Guid.NewGuid());

        await inMemoryBus.RaiseEventAsync(userDeletedEvent);

        await mediator.Received(1).Publish(Arg.Is<UserDeletedEvent>(x => x.Equals(userDeletedEvent)));
    }

    [Fact]
    public async Task InMemoryBus_Should_Send_Command_Async()
    {
        var mediator = Substitute.For<IMediator>();
        var domainEventStore = Substitute.For<IDomainEventStore>();
        var fanoutEventHandler = Substitute.For<IFanoutEventHandler>();

        var inMemoryBus = new InMemoryBus(mediator, domainEventStore, fanoutEventHandler);

        var deleteUserCommand = new DeleteUserCommand(Guid.NewGuid());

        await inMemoryBus.SendCommandAsync(deleteUserCommand);

        await mediator.Received(1).Send(Arg.Is<DeleteUserCommand>(x => x.Equals(deleteUserCommand)));
    }
}