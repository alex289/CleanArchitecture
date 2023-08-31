using System.Collections.Generic;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries.Tenants.GetAll;
using CleanArchitecture.Application.Queries.Tenants.GetTenantById;
using CleanArchitecture.Application.Queries.Users.GetAll;
using CleanArchitecture.Application.Queries.Users.GetUserById;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Application.ViewModels.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITenantService, TenantService>();

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        // User
        services.AddScoped<IRequestHandler<GetUserByIdQuery, UserViewModel?>, GetUserByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllUsersQuery, IEnumerable<UserViewModel>>, GetAllUsersQueryHandler>();

        // Tenant
        services.AddScoped<IRequestHandler<GetTenantByIdQuery, TenantViewModel?>, GetTenantByIdQueryHandler>();
        services
            .AddScoped<IRequestHandler<GetAllTenantsQuery, IEnumerable<TenantViewModel>>, GetAllTenantsQueryHandler>();

        return services;
    }
}