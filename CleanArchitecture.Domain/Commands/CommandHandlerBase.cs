using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Notifications;
using MediatR;

namespace CleanArchitecture.Domain.Commands;

public abstract class CommandHandlerBase
{
    protected readonly IMediatorHandler _bus;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DomainNotificationHandler _notifications;

    protected CommandHandlerBase(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications)
    {
        _bus = bus;
        _unitOfWork = unitOfWork;
        _notifications = (DomainNotificationHandler)notifications;
    }
    
    public async Task<bool> CommitAsync()
    {
        if (_notifications.HasNotifications())
        {
            return false;
        }

        if (await _unitOfWork.CommitAsync())
        {
            return true;
        }

        await _bus.RaiseEventAsync(
            new DomainNotification(
                "Commit",
                "Problem occured while saving the data. Please try again.",
                ErrorCodes.CommitFailed));

        return false;
    }
    
    protected async Task NotifyAsync(string key, string message, string code)
    {
        await _bus.RaiseEventAsync(
            new DomainNotification(key, message, code));
    }

    protected async Task NotifyAsync(DomainNotification notification)
    {
        await _bus.RaiseEventAsync(notification);
    }
    
    protected async ValueTask<bool> TestValidityAsync(CommandBase command)
    {
        if (command.IsValid())
        {
            return true;
        }

        if (command.ValidationResult == null)
        {
            throw new InvalidOperationException("Command is invalid and should therefore have a validation result");
        }

        foreach (var error in command.ValidationResult!.Errors)
        {
            await NotifyAsync(
                new DomainNotification(
                    command.MessageType, 
                    error.ErrorMessage, 
                    error.ErrorCode,
                    error.FormattedMessagePlaceholderValues));
        }

        return false;
    }
}