using CleanArchitecture.Domain.DomainEvents;
using CleanArchitecture.Infrastructure.Configurations.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database;

public class EventStoreDbContext : DbContext
{
    public virtual DbSet<StoredDomainEvent> StoredDomainEvents { get; set; } = null!;

    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StoredDomainEventConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}