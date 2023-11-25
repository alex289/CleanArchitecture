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
    private readonly IMediatorHandler _bus;
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMediatorHandler bus)
    {
        _userRepository = userRepository;
        _bus = bus;
    }

    public async Task<UserViewModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user is null)
        {
            await _bus.RaiseEventAsync(
                new DomainNotification(
                    nameof(GetUserByIdQuery),
                    $"User with id {request.Id} could not be found",
                    ErrorCodes.ObjectNotFound));
            return null;
        }

        return UserViewModel.FromUser(user);
    }
}