using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Extensions;

public static class DbContextExtension
{
    public static void EnsureMigrationsApplied(this DbContext context)
    {
        var applied = context.GetService<IHistoryRepository>().GetAppliedMigrations().Select(m => m.MigrationId);

        var total = context.GetService<IMigrationsAssembly>().Migrations.Select(m => m.Key);

        if (total.Except(applied).Any())
        {
            context.Database.Migrate();
        }
    }
}