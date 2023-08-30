using CleanArchitecture.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations.EventSourcing;

public sealed class StoredDomainEventConfiguration : IEntityTypeConfiguration<StoredDomainEvent>
{
    public void Configure(EntityTypeBuilder<StoredDomainEvent> builder)
    {
        builder.Property(c => c.Timestamp)
            .HasColumnName("CreationDate");

        builder.Property(c => c.MessageType)
            .HasColumnName("Action")
            .HasColumnType("varchar(100)");

        builder.Property(c => c.CorrelationId)
            .HasMaxLength(100);

        builder.Property(c => c.User)
            .HasMaxLength(100)
            .HasColumnType("nvarchar(100)");
    }
}