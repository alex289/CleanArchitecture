using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Events.User;
using MediatR;

namespace CleanArchitecture.Domain.EventHandler;

public sealed class UserEventHandler :
    INotificationHandler<UserDeletedEvent>
{
    public Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}