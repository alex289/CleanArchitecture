using CleanArchitecture.Domain.DomainNotifications;
using CleanArchitecture.Infrastructure.Configurations.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database;

public class DomainNotificationStoreDbContext : DbContext
{
    public DomainNotificationStoreDbContext(DbContextOptions<DomainNotificationStoreDbContext> options) : base(options)
    {
    }

    public virtual DbSet<StoredDomainNotification> StoredDomainNotifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new StoredDomainNotificationConfiguration());
    }
}