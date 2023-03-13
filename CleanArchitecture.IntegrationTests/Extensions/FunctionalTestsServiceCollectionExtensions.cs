using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.IntegrationTests.Extensions;

public static class FunctionalTestsServiceCollectionExtensions
{
    public static IServiceCollection SetupTestDatabase<TContext>(this IServiceCollection services, DbConnection connection) where TContext : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TContext>));
        if (descriptor != null)
            services.Remove(descriptor);

        services.AddScoped(p =>
        DbContextOptionsFactory<TContext>(
        p,
        (_, options) => options
            .ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
            .UseLazyLoadingProxies()
            .UseSqlite(connection)));

        return services;
    }

    private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
        IServiceProvider applicationServiceProvider,
        Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where TContext : DbContext
    {
        var builder = new DbContextOptionsBuilder<TContext>(
            new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));

        builder.UseApplicationServiceProvider(applicationServiceProvider);

        optionsAction.Invoke(applicationServiceProvider, builder);

        return builder.Options;
    }
}
