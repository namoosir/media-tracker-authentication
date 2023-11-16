using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerAuthenticationService.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtColumnForPlatformConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlatformConnections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PlatformConnections");
        }
    }
}
