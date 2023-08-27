using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder
            .Property(user => user.Name)
            .IsRequired()
            .HasMaxLength(MaxLengths.Tenant.Name);

        builder.HasData(new Tenant(
            Ids.Seed.TenantId,
            "Admin Tenant"));
    }
}