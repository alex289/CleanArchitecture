using System.Linq;
using System.Net;
using CleanArchitecture.Api.Models;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

public class ApiController : ControllerBase
{
    private readonly DomainNotificationHandler _notifications;

    protected ApiController(INotificationHandler<DomainNotification> notifications)
    {
        _notifications = (DomainNotificationHandler)notifications;
    }

    protected new IActionResult Response(object? resultData = null)
    {
        if (!_notifications.HasNotifications())
        {
            return Ok(
                new ResponseMessage<object>
                {
                    Success = true,
                    Data = resultData
                });
        }

        var message = new ResponseMessage<object>
        {
            Success = false,
            Errors = _notifications.GetNotifications().Select(n => n.Value),
            DetailedErrors = _notifications.GetNotifications().Select(n => new DetailedError
            {
                Code = n.Code,
                Data = n.Data
            })
        };

        return new ObjectResult(message)
        {
            StatusCode = (int)GetErrorStatusCode()
        };
    }

    protected HttpStatusCode GetStatusCode()
    {
        if (!_notifications.GetNotifications().Any())
        {
            return HttpStatusCode.OK;
        }

        return GetErrorStatusCode();
    }

    private HttpStatusCode GetErrorStatusCode()
    {
        if (_notifications.GetNotifications().Any(n => n.Code == ErrorCodes.ObjectNotFound))
        {
            return HttpStatusCode.NotFound;
        }

        if (_notifications.GetNotifications().Any(n => n.Code == ErrorCodes.InsufficientPermissions))
        {
            return HttpStatusCode.Forbidden;
        }

        return HttpStatusCode.BadRequest;
    }
}