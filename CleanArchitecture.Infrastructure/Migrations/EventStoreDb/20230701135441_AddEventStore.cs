#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Migrations.EventStoreDb;

/// <inheritdoc />
public partial class AddEventStore : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "StoredDomainEvents",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Data = table.Column<string>("nvarchar(max)", nullable: false),
                User = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CorrelationId = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                AggregateId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Action = table.Column<string>("varchar(100)", nullable: false),
                CreationDate = table.Column<DateTime>("datetime2", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_StoredDomainEvents", x => x.Id); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "StoredDomainEvents");
    }
}