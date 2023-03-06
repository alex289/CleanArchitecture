using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder
            .Property(user => user.GivenName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(user => user.Surname)
            .IsRequired()
            .HasMaxLength(100);
    }
}