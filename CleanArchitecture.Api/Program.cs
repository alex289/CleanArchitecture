using System;
using CleanArchitecture.Api.BackgroundServices;
using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Application.gRPC;
using CleanArchitecture.Domain.Extensions;
using CleanArchitecture.Domain.Rabbitmq.Extensions;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.ServiceDefaults;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddApplicationStatus();

var isAspire = builder.Configuration["ASPIRE_ENABLED"] == "true";

var rabbitConfiguration = builder.Configuration.GetRabbitMqConfiguration();
var redisConnectionString =
    isAspire ? builder.Configuration["ConnectionStrings:Redis"] : builder.Configuration["RedisHostName"];
var dbConnectionString = isAspire
    ? builder.Configuration["ConnectionStrings:Database"]
    : builder.Configuration["ConnectionStrings:DefaultConnection"];

if (builder.Environment.IsProduction())
{
    builder.Services
        .AddHealthChecks()
        .AddSqlServer(dbConnectionString!)
        .AddRedis(redisConnectionString!, "Redis")
        .AddRabbitMQ(
            async _ =>
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(rabbitConfiguration.ConnectionString),
                };
                return await factory.CreateConnectionAsync();
            },
            name: "RabbitMQ");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(dbConnectionString,
        b => b.MigrationsAssembly("CleanArchitecture.Infrastructure"));
});

builder.Services.AddSwagger();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddInfrastructure("CleanArchitecture.Infrastructure", dbConnectionString!);
builder.Services.AddQueryHandlers();
builder.Services.AddServices();
builder.Services.AddSortProviders();
builder.Services.AddCommandHandlers();
builder.Services.AddNotificationHandlers();
builder.Services.AddApiUser();

builder.Services.AddRabbitMqHandler(rabbitConfiguration);

builder.Services.AddHostedService<SetInactiveUsersService>();

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly); });

builder.Services.AddLogging(x => x.AddSimpleConsole(console =>
{
    console.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss.fff] ";
    console.IncludeScopes = true;
}));

if (builder.Environment.IsProduction() || !string.IsNullOrWhiteSpace(redisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "clean-architecture";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var appDbContext = services.GetRequiredService<ApplicationDbContext>();
    var storeDbContext = services.GetRequiredService<EventStoreDbContext>();
    var domainStoreDbContext = services.GetRequiredService<DomainNotificationStoreDbContext>();

    appDbContext.EnsureMigrationsApplied();
    storeDbContext.EnsureMigrationsApplied();
    domainStoreDbContext.EnsureMigrationsApplied();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();
app.MapGrpcService<UsersApiImplementation>();
app.MapGrpcService<TenantsApiImplementation>();

app.Run();

// Needed for integration tests web application factory
public partial class Program
{
}