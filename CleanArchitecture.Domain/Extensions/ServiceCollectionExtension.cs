using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.EventHandler;
using CleanArchitecture.Domain.Events.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Domain.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        // User
        services.AddScoped<IRequestHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        
        return services;
    }
    
    public static IServiceCollection AddNotificationHandlers(this IServiceCollection services)
    {
        // User
        services.AddScoped<INotificationHandler<UserDeletedEvent>, UserEventHandler>();
        
        return services;
    }
}