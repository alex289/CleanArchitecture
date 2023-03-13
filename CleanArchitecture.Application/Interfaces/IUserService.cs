using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using CleanArchitecture.Application.ViewModels.Users;

namespace CleanArchitecture.Application.Interfaces;

public interface IUserService
{
    public Task<UserViewModel?> GetUserByUserIdAsync(Guid userId, bool isDeleted);
    public Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
    public Task<Guid> CreateUserAsync(CreateUserViewModel user);
    public Task UpdateUserAsync(UpdateUserViewModel user);
    public Task DeleteUserAsync(Guid userId);
}