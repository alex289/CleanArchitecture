using System;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ApiController
{
    public UserController(NotificationHandler<DomainNotification> notifications) : base(notifications)
    {
    }

    [HttpGet]
    public string GetAllUsersAsync()
    {
        return "test";
    }
    
    [HttpGet("{id}")]
    public string GetUserByIdAsync([FromRoute] Guid id)
    {
        return "test";
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