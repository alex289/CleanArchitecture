using System;
using Aikido.Zen.DotNetCore;
using CleanArchitecture.Api.BackgroundServices;
using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Application.gRPC;
using CleanArchitecture.Domain.Consumers;
using CleanArchitecture.Domain.Extensions;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.ServiceDefaults;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsProduction())
{
    builder.Services.AddZenFirewall();
}

var isAspire = builder.Configuration["ASPIRE_ENABLED"] == "true";

var rabbitConfiguration = builder.Configuration.GetRabbitMqConfiguration();
var redisConnectionString =
    isAspire ? builder.Configuration["ConnectionStrings:Redis"] : builder.Configuration["RedisHostName"];
var dbConnectionString = isAspire
    ? builder.Configuration["ConnectionStrings:Database"]
    : builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddApplicationStatus()
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

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<FanoutEventConsumer>();
    x.AddConsumer<TenantUpdatedEventConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureNewtonsoftJsonSerializer(settings =>
        {
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return settings;
        });
        cfg.UseNewtonsoftJsonSerializer();
        cfg.ConfigureNewtonsoftJsonDeserializer(settings =>
        {
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return settings;
        });
        
        cfg.Host(rabbitConfiguration.Host, (ushort)rabbitConfiguration.Port, "/", h => {
            h.Username(rabbitConfiguration.Username);
            h.Password(rabbitConfiguration.Password);
        });

        // Every instance of the service will receive the message
        cfg.ReceiveEndpoint("clean-architecture-fanout-event-" + Guid.NewGuid(), e =>
        {
            e.Durable = false;
            e.AutoDelete = true;
            e.ConfigureConsumer<FanoutEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ReceiveEndpoint("clean-architecture-fanout-events", e =>
        {
            e.ConfigureConsumer<TenantUpdatedEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
        cfg.ConfigureEndpoints(context);
    });
});

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

if (builder.Environment.IsProduction())
{
    app.UseZenFirewall();
}

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();
app.MapGrpcService<UsersApiImplementation>();
app.MapGrpcService<TenantsApiImplementation>();

await app.RunAsync();

// Needed for integration tests web application factory
public partial class Program
{
}