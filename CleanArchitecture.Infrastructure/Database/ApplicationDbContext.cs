using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database;

public partial class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            if (entity.ClrType.GetProperty(DbContextUtility.IsDeletedProperty) is not null)
            {
                builder.Entity(entity.ClrType)
                    .HasQueryFilter(DbContextUtility.GetIsDeletedRestriction(entity.ClrType));
            }
        }

        base.OnModelCreating(builder);

        ApplyConfigurations(builder);

        // Make referential delete behaviour restrict instead of cascade for everything
        foreach (var relationship in builder.Model.GetEntityTypes()
                     .SelectMany(x => x.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private static void ApplyConfigurations(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new TenantConfiguration());
    }
}