using System;
using System.Linq;
using System.Security.Claims;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Domain;

public sealed class ApiUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.NameIdentifier));

        if (Guid.TryParse(claim?.Value, out var userId))
        {
            return userId;
        }

        throw new ArgumentException("Could not parse user id to guid");
    }

    public UserRole GetUserRole()
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Role));

        if (Enum.TryParse(claim?.Value, out UserRole userRole))
        {
            return userRole;
        }

        throw new ArgumentException("Could not parse user role");
    }

    public string Name => _httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;

    public string GetUserEmail()
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Email));

        if (!string.IsNullOrWhiteSpace(claim?.Value))
        {
            return claim.Value;
        }

        throw new ArgumentException("Could not parse user email");
    }
}