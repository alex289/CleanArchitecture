using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Api.Models;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class UserController : ApiController
{
    private readonly IUserService _userService;

    public UserController(
        INotificationHandler<DomainNotification> notifications,
        IUserService userService) : base(notifications)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    [SwaggerOperation("Get a list of all users")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<IEnumerable<UserViewModel>>))]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Response(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    [SwaggerOperation("Get a user by id")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UserViewModel>))]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromRoute] Guid id,
        [FromQuery] bool isDeleted = false)
    {
        var user = await _userService.GetUserByUserIdAsync(id, isDeleted);
        return Response(user);
    }
    
    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation("Get the current active user")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UserViewModel>))]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var user = await _userService.GetCurrentUserAsync();
        return Response(user);
    }
    
    [HttpPost]
    [SwaggerOperation("Create a new user")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserViewModel viewModel)
    {
        var userId = await _userService.CreateUserAsync(viewModel);
        return Response(userId);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a user")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return Response(id);
    }

    [Authorize]
    [HttpPut]
    [SwaggerOperation("Update a user")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UpdateUserViewModel>))]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserViewModel viewModel)
    {
        await _userService.UpdateUserAsync(viewModel);
        return Response(viewModel);
    }

    [Authorize]
    [HttpPost("changePassword")]
    [SwaggerOperation("Change a password for the current active user")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<ChangePasswordViewModel>))]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel viewModel)
    {
        await _userService.ChangePasswordAsync(viewModel);
        return Response(viewModel);
    }

    [HttpPost("login")]
    [SwaggerOperation("Get a signed token for a user")]
    [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<string>))]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserViewModel viewModel)
    {
        var token = await _userService.LoginUserAsync(viewModel);
        return Response(token);
    }
}