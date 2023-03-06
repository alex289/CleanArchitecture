using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Domain.Interfaces;

namespace CleanArchitecture.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IMediatorHandler _bus;

    public UserService(IMediatorHandler bus)
    {
        _bus = bus;
    }

    public async Task<UserViewModel?> GetUserByUserIdAsync(Guid userId)
    {
        return await _bus.QueryAsync(new GetUserByIdQuery(userId));
    }
}