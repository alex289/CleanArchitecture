using CleanArchitecture.Domain.Constants;
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
            .HasMaxLength(MaxLengths.User.Email);

        builder
            .Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(MaxLengths.User.FirstName);

        builder
            .Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(MaxLengths.User.LastName);

        builder
            .Property(user => user.Password)
            .IsRequired()
            .HasMaxLength(MaxLengths.User.Password);

        builder.HasData(new User(
            Ids.Seed.UserId,
            Ids.Seed.TenantId,
            "admin@email.com",
            "Admin",
            "User",
            // !Password123#
            "$2a$12$Blal/uiFIJdYsCLTMUik/egLbfg3XhbnxBC6Sb5IKz2ZYhiU/MzL2",
            UserRole.Admin));
    }
}