using System;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.EventSourcing;

public sealed class EventStoreContext : IEventStoreContext
{
    private readonly string _correlationId;
    private readonly IUser? _user;

    public EventStoreContext(IUser? user, IHttpContextAccessor? httpContextAccessor)
    {
        _user = user;

        if (httpContextAccessor?.HttpContext is null ||
            !httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-CLEAN-ARCHITECTURE-CORRELATION-ID",
                out var id))
        {
            _correlationId = $"internal - {Guid.NewGuid()}";
        }
        else
        {
            _correlationId = id.ToString();
        }
    }

    public string GetCorrelationId()
    {
        return _correlationId;
    }

    public string GetUserEmail()
    {
        return _user?.GetUserEmail() ?? string.Empty;
    }
}