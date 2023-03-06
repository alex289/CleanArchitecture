using CleanArchitecture.Application.ViewModels;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Interfaces;

public interface IUserService
{
    public Task<UserViewModel?> GetUserByUserIdAsync(Guid userId);
    public Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
}