using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Queries.Users.GetAll;

public sealed class GetAllUsersQueryHandler :
    IRequestHandler<GetAllUsersQuery, PagedResult<UserViewModel>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<UserViewModel>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var usersQuery = _userRepository
            .GetAllNoTracking()
            .Where(x => !x.Deleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            usersQuery = usersQuery.Where(user => 
                user.Email.Contains(request.SearchTerm) ||
                user.FirstName.Contains(request.SearchTerm) ||
                user.LastName.Contains(request.SearchTerm));
        }
        
        var totalCount = await usersQuery.CountAsync(cancellationToken);
        
        var users = await usersQuery
            .Skip((request.Query.Page - 1) * request.Query.PageSize)
            .Take(request.Query.PageSize)
            .Select(user => UserViewModel.FromUser(user))
            .ToListAsync(cancellationToken);

        return new PagedResult<UserViewModel>(
            totalCount, users, request.Query.Page, request.Query.PageSize);
    }
}