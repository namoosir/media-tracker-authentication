using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaTrackerAuthenticationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformConnections",
                columns: table =>
                    new
                    {
                        UserId = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Platform = table.Column<int>(type: "int", nullable: false),
                        AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Scopes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformConnections", x => x.UserId);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PlatformConnections");
        }
    }
}
