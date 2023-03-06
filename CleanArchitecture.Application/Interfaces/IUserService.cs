using CleanArchitecture.Application.ViewModels;
using System.Threading.Tasks;
using System;

namespace CleanArchitecture.Application.Interfaces;

public interface IUserService
{
    public Task<UserViewModel?> GetUserByUserIdAsync(Guid userId);
}