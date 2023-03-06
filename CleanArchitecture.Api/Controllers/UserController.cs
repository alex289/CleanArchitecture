using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Interfaces;
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
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
    {
        var user = await _userService.GetUserByUserIdAsync(id);
        return Response(user);
    }
    
    [HttpPost]
    public string CreateUserAsync()
    {
        return "test";
    }
    
    [HttpDelete("{id}")]
    public string DeleteUserAsync([FromRoute] Guid id)
    {
        return "test";
    }
    
    [HttpPut]
    public string UpdateUserAsync()
    {
        return "test";
    }
}