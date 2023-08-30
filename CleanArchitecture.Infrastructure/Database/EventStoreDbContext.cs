using CleanArchitecture.Domain.DomainEvents;
using CleanArchitecture.Infrastructure.Configurations.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database;

public class EventStoreDbContext : DbContext
{
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
    {
    }

    public virtual DbSet<StoredDomainEvent> StoredDomainEvents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StoredDomainEventConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}