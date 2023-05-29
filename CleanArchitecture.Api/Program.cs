using System;
using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Application.gRPC;
using CleanArchitecture.Domain.Extensions;
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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddApplicationStatus();

if (builder.Environment.IsProduction())
{
    builder.Services
        .AddHealthChecks()
        .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CleanArchitecture.Infrastructure"));
});

builder.Services.AddSwagger();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddQueryHandlers();
builder.Services.AddServices();
builder.Services.AddCommandHandlers();
builder.Services.AddNotificationHandlers();
builder.Services.AddApiUser();

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapGrpcService<UsersApiImplementation>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var appDbContext = services.GetRequiredService<ApplicationDbContext>();

    appDbContext.EnsureMigrationsApplied();
}

app.Run();

// Needed for integration tests web application factory
public partial class Program
{
}