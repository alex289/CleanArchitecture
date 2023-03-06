using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
    
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        // services.AddScoped<IQueryHandler<GetUserByIdQuery, User>, GetUserByIdQueryHandler>();
        
        return services;
    }
}