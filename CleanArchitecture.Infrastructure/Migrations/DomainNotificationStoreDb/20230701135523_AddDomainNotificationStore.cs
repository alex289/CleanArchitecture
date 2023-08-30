#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Migrations.DomainNotificationStoreDb;

/// <inheritdoc />
public partial class AddDomainNotificationStore : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "StoredDomainNotifications",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Data = table.Column<string>("nvarchar(max)", nullable: false),
                User = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CorrelationId = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                AggregateId = table.Column<Guid>("uniqueidentifier", nullable: false),
                MessageType = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Timestamp = table.Column<DateTime>("datetime2", nullable: false),
                Key = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Value = table.Column<string>("nvarchar(1024)", maxLength: 1024, nullable: false),
                Code = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Version = table.Column<int>("int", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_StoredDomainNotifications", x => x.Id); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "StoredDomainNotifications");
    }
}