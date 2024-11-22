using CleanArchitecture.Domain.DomainEvents;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.EventSourcing;
using CleanArchitecture.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string migrationsAssemblyName,
        string connectionString)
    {
        // Add event store db context
        services.AddDbContext<EventStoreDbContext>(
            options =>
            {
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(migrationsAssemblyName));
            });

        services.AddDbContext<DomainNotificationStoreDbContext>(
            options =>
            {
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(migrationsAssemblyName));
            });

        // Core Infra
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
        services.AddScoped<IEventStoreContext, EventStoreContext>();
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        services.AddScoped<IDomainEventStore, DomainEventStore>();
        services.AddScoped<IMediatorHandler, InMemoryBus>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();

        return services;
    }
}