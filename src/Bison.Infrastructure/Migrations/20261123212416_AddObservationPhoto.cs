using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddObservationPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Post",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Post");
        }
    }
}
