using System;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
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
            .Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(user => user.Password)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasData(new User(
            Guid.NewGuid(),
            "admin@email.com",
            "Admin",
            "User",
            // !Password123#
            "$2a$12$Blal/uiFIJdYsCLTMUik/egLbfg3XhbnxBC6Sb5IKz2ZYhiU/MzL2",
            UserRole.Admin));
    }
}