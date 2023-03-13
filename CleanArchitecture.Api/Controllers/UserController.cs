using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ApiController
{
    private readonly IUserService _userService;

    public UserController(
        INotificationHandler<DomainNotification> notifications,
        IUserService userService) : base(notifications)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Response(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromRoute] Guid id,
        [FromQuery] bool isDeleted = false)
    {
        var user = await _userService.GetUserByUserIdAsync(id, isDeleted);
        return Response(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserViewModel viewModel)
    {
        var userId = await _userService.CreateUserAsync(viewModel);
        return Response(userId);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return Response(id);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserViewModel viewModel)
    {
        await _userService.UpdateUserAsync(viewModel);
        return Response(viewModel);
    }
}