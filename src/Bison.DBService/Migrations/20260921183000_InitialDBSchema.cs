using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.DBService.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommentVal = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "proposals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaxonID = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proposals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "readings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Observation = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_readings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "taxons",
                columns: table => new
                {
                    TaxonId = table.Column<string>(type: "TEXT", nullable: false),
                    ParentTaxonId = table.Column<string>(type: "TEXT", nullable: true),
                    TaxonRank = table.Column<string>(type: "TEXT", nullable: false),
                    ScientificName = table.Column<string>(type: "TEXT", nullable: false),
                    VernacularName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxons", x => x.TaxonId);
                    table.ForeignKey(
                        name: "FK_taxons_taxons_ParentTaxonId",
                        column: x => x.ParentTaxonId,
                        principalTable: "taxons",
                        principalColumn: "TaxonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_taxons_ParentTaxonId",
                table: "taxons",
                column: "ParentTaxonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "proposals");

            migrationBuilder.DropTable(
                name: "readings");

            migrationBuilder.DropTable(
                name: "taxons");
        }
    }
}
