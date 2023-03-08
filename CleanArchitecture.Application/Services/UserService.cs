using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Commands.Users.CreateUser;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
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

    public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
    {
        return await _bus.QueryAsync(new GetAllUsersQuery());
    }
    
    public async Task CreateUserAsync(CreateUserViewModel user)
    {
        await _bus.SendCommandAsync(new CreateUserCommand(
            Guid.NewGuid(),
            user.Email,
            user.Surname,
            user.GivenName));
    }
    
    public async Task UpdateUserAsync(UpdateUserViewModel user)
    {
        await _bus.SendCommandAsync(new UpdateUserCommand(
            user.Id,
            user.Email,
            user.Surname,
            user.GivenName));
    }
    
    public async Task DeleteUserAsync(Guid userId)
    {
        await _bus.SendCommandAsync(new DeleteUserCommand(userId));
    }
}