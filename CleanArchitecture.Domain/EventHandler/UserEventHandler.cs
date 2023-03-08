using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Events.User;
using MediatR;

namespace CleanArchitecture.Domain.EventHandler;

public sealed class UserEventHandler :
    INotificationHandler<UserDeletedEvent>,
    INotificationHandler<UserCreatedEvent>,
    INotificationHandler<UserUpdatedEvent>
{
    public Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}