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

    public async Task<UserViewModel?> GetUserByUserIdAsync(Guid userId, bool isDeleted)
    {
        return await _bus.QueryAsync(new GetUserByIdQuery(userId, isDeleted));
    }

    public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
    {
        return await _bus.QueryAsync(new GetAllUsersQuery());
    }
    
    public async Task<Guid> CreateUserAsync(CreateUserViewModel user)
    {
        var userId = Guid.NewGuid();

        await _bus.SendCommandAsync(new CreateUserCommand(
            userId,
            user.Email,
            user.Surname,
            user.GivenName));

        return userId;
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