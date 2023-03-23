using CleanArchitecture.gRPC.Contexts;
using CleanArchitecture.gRPC.Interfaces;
using CleanArchitecture.gRPC.Models;
using CleanArchitecture.Proto.Users;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.gRPC.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionKey = "gRPC")
    {
        var settings = new GRPCSettings();
        configuration.Bind(configSectionKey, settings);

        return AddGrpcClient(services, settings);
    }

    public static IServiceCollection AddGrpcClient(this IServiceCollection services, GRPCSettings settings)
    {
        if (!string.IsNullOrWhiteSpace(settings.CleanArchitectureUrl))
        {
            services.AddCleanArchitectureGrpcClient(settings.CleanArchitectureUrl);
        }

        services.AddSingleton<ICleanArchitecture, CleanArchitecture>();

        return services;
    }

    public static IServiceCollection AddCleanArchitectureGrpcClient(
        this IServiceCollection services,
        string tetraQueryApiUrl)
    {
        if (string.IsNullOrWhiteSpace(tetraQueryApiUrl))
        {
            return services;
        }

        var channel = GrpcChannel.ForAddress(tetraQueryApiUrl);

        var usersClient = new UsersApi.UsersApiClient(channel);
        services.AddSingleton(usersClient);

        services.AddSingleton<IUsersContext, UsersContext>();

        return services;
    }
}
