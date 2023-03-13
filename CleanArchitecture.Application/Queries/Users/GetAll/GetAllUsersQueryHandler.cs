using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Queries.Users.GetAll;

public sealed class GetAllUsersQueryHandler :
    IRequestHandler<GetAllUsersQuery, IEnumerable<UserViewModel>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository
            .GetAllNoTracking()
            .Where(x => !x.Deleted)
            .Select(x => UserViewModel.FromUser(x))
            .ToListAsync(cancellationToken);
    }
}