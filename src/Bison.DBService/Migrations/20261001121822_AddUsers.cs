using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.DBService.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "readings");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "readings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_readings_UserId",
                table: "readings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_readings_users_UserId",
                table: "readings",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_readings_users_UserId",
                table: "readings");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_readings_UserId",
                table: "readings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "readings");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "readings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
