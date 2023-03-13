using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Domain.Extensions;
using CleanArchitecture.gRPC;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CleanArchitecture.Infrastructure"));
});

builder.Services.AddInfrastructure();
builder.Services.AddQueryHandlers();
builder.Services.AddServices();
builder.Services.AddCommandHandlers();
builder.Services.AddNotificationHandlers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<UsersApiImplementation>();

using (IServiceScope scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    ApplicationDbContext appDbContext = services.GetRequiredService<ApplicationDbContext>();

    appDbContext.EnsureMigrationsApplied();
}

app.Run();

// Needed for integration tests webapplication factory
public partial class Program { }