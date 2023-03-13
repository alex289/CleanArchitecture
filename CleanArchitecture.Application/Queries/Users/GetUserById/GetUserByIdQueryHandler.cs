using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Application.Queries.Users.GetUserById;

public sealed class GetUserByIdQueryHandler :
    IRequestHandler<GetUserByIdQuery, UserViewModel?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMediatorHandler _bus;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMediatorHandler bus)
    {
        _userRepository = userRepository;
        _bus = bus;
    }

    public async Task<UserViewModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = _userRepository
            .GetAllNoTracking()
            .FirstOrDefault(x => 
                x.Id == request.UserId &&
                x.Deleted == request.IsDeleted);

        if (user == null)
        {
            await _bus.RaiseEventAsync(
                new DomainNotification(
                    nameof(GetUserByIdQuery),
                    $"User with id {request.UserId} could not be found",
                    ErrorCodes.ObjectNotFound));
            return null;
        }

        return UserViewModel.FromUser(user);
    }
}
