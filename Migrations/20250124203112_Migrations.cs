using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamWebRtc.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConnectionID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IceServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: true),
                    credential = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IceServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stream",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    urlStream = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stream", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StunServers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Urls = table.Column<string>(type: "TEXT", nullable: true),
                    IceServerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StunServers", x => x.id);
                    table.ForeignKey(
                        name: "FK_StunServers_IceServers_IceServerId",
                        column: x => x.IceServerId,
                        principalTable: "IceServers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TurnServers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Urls = table.Column<string>(type: "TEXT", nullable: true),
                    IceServerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnServers", x => x.id);
                    table.ForeignKey(
                        name: "FK_TurnServers_IceServers_IceServerId",
                        column: x => x.IceServerId,
                        principalTable: "IceServers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StunServers_IceServerId",
                table: "StunServers",
                column: "IceServerId");

            migrationBuilder.CreateIndex(
                name: "IX_TurnServers_IceServerId",
                table: "TurnServers",
                column: "IceServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cams");

            migrationBuilder.DropTable(
                name: "Stream");

            migrationBuilder.DropTable(
                name: "StunServers");

            migrationBuilder.DropTable(
                name: "TurnServers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "IceServers");
        }
    }
}
