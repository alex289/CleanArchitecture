using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Users;

namespace CleanArchitecture.Application.Interfaces;

public interface IUserService
{
    public Task<UserViewModel?> GetUserByUserIdAsync(Guid userId, bool isDeleted);
    public Task<UserViewModel?> GetCurrentUserAsync();
    public Task<PagedResult<UserViewModel>> GetAllUsersAsync(PageQuery query, string searchTerm = "");
    public Task<Guid> CreateUserAsync(CreateUserViewModel user);
    public Task UpdateUserAsync(UpdateUserViewModel user);
    public Task DeleteUserAsync(Guid userId);
    public Task ChangePasswordAsync(ChangePasswordViewModel viewModel);
    public Task<string> LoginUserAsync(LoginUserViewModel viewModel);
}