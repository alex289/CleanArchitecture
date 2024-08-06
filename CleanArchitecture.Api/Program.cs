using CleanArchitecture.Api.BackgroundServices;
using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Application.gRPC;
using CleanArchitecture.Domain.Extensions;
using CleanArchitecture.Domain.Rabbitmq.Extensions;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Extensions;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddApplicationStatus();

if (builder.Environment.IsProduction())
{
    var rabbitHost = builder.Configuration["RabbitMQ:Host"];
    var rabbitPort = builder.Configuration["RabbitMQ:Port"];
    var rabbitUser = builder.Configuration["RabbitMQ:Username"];
    var rabbitPass = builder.Configuration["RabbitMQ:Password"];

    builder.Services
        .AddHealthChecks()
        .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!)
        .AddRedis(builder.Configuration["RedisHostName"]!, "Redis")
        .AddRabbitMQ(
            $"amqp://{rabbitUser}:{rabbitPass}@{rabbitHost}:{rabbitPort}",
            name: "RabbitMQ");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CleanArchitecture.Infrastructure"));
});

builder.Services.AddSwagger();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, "CleanArchitecture.Infrastructure");
builder.Services.AddQueryHandlers();
builder.Services.AddServices();
builder.Services.AddSortProviders();
builder.Services.AddCommandHandlers();
builder.Services.AddNotificationHandlers();
builder.Services.AddApiUser();

builder.Services.AddRabbitMqHandler(builder.Configuration, "RabbitMQ");

builder.Services.AddHostedService<SetInactiveUsersService>();

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly); });

builder.Services.AddLogging(x => x.AddSimpleConsole(console =>
{
    console.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss.fff] ";
    console.IncludeScopes = true;
}));

if (builder.Environment.IsProduction() || !string.IsNullOrWhiteSpace(builder.Configuration["RedisHostName"]))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["RedisHostName"];
        options.InstanceName = "clean-architecture";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

var app = builder.Build();

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