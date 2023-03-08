using CleanArchitecture.Domain.Commands.Users.CreateUser;
using CleanArchitecture.Domain.Commands.Users.DeleteUser;
using CleanArchitecture.Domain.Commands.Users.UpdateUser;
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
        services.AddScoped<IRequestHandler<CreateUserCommand>, CreateUserCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        
        return services;
    }
    
    public static IServiceCollection AddNotificationHandlers(this IServiceCollection services)
    {
        // User
        services.AddScoped<INotificationHandler<UserCreatedEvent>, UserEventHandler>();
        services.AddScoped<INotificationHandler<UserUpdatedEvent>, UserEventHandler>();
        services.AddScoped<INotificationHandler<UserDeletedEvent>, UserEventHandler>();
        
        return services;
    }
}