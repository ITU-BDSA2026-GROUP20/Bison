using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattlePass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattlePassTier",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "BattlePassUpgraded",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "battlePass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tier = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    PrimaryColor = table.Column<uint>(type: "INTEGER", nullable: true),
                    SecondaryColor = table.Column<uint>(type: "INTEGER", nullable: true),
                    BackgroundColor = table.Column<uint>(type: "INTEGER", nullable: true),
                    TextColor = table.Column<uint>(type: "INTEGER", nullable: true),
                    Feature = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battlePass", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_battlePass_Tier_IsPaid",
                table: "battlePass",
                columns: new[] { "Tier", "IsPaid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battlePass");

            migrationBuilder.DropColumn(
                name: "BattlePassTier",
                table: "users");

            migrationBuilder.DropColumn(
                name: "BattlePassUpgraded",
                table: "users");
        }
    }
}
