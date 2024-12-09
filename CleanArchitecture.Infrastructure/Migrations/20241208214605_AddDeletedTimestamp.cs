using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Tenants",
                type: "datetimeoffset",
                nullable: true);
            
            migrationBuilder.Sql("UPDATE Users SET DeletedAt = SYSDATETIMEOFFSET() WHERE Deleted = 1");
            migrationBuilder.Sql("UPDATE Tenants SET DeletedAt = SYSDATETIMEOFFSET() WHERE Deleted = 1");
            
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Tenants");

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("b542bf25-134c-47a2-a0df-84ed14d03c4a"),
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                column: "DeletedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.Sql("UPDATE Users SET Deleted = true WHERE DeletedAt IS NOT NULL");
            migrationBuilder.Sql("UPDATE Tenants SET Deleted = true WHERE DeletedAt IS NOT NULL");
            
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tenants");

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("b542bf25-134c-47a2-a0df-84ed14d03c4a"),
                column: "Deleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e3892c0-9374-49fa-a3fd-53db637a40ae"),
                column: "Deleted",
                value: false);
        }
    }
}
