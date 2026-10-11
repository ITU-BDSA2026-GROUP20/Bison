using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.DBService.Migrations
{
    /// <inheritdoc />
    public partial class TaxonIntKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_taxons_TaxonId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_taxons_taxons_ParentTaxonId",
                table: "taxons");

            migrationBuilder.DropIndex(
                name: "IX_taxons_ParentTaxonId",
                table: "taxons");

            migrationBuilder.DropIndex(
                name: "IX_Post_TaxonId",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "TaxonId",
                table: "taxons",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ParentTaxonId1",
                table: "taxons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dwc_TaxonID",
                table: "taxons",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Proposal_TaxonId",
                table: "Post",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxonId1",
                table: "Post",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_taxons_ParentTaxonId1",
                table: "taxons",
                column: "ParentTaxonId1");

            migrationBuilder.CreateIndex(
                name: "IX_Post_TaxonId1",
                table: "Post",
                column: "TaxonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_taxons_TaxonId1",
                table: "Post",
                column: "TaxonId1",
                principalTable: "taxons",
                principalColumn: "TaxonId");

            migrationBuilder.AddForeignKey(
                name: "FK_taxons_taxons_ParentTaxonId1",
                table: "taxons",
                column: "ParentTaxonId1",
                principalTable: "taxons",
                principalColumn: "TaxonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_taxons_TaxonId1",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_taxons_taxons_ParentTaxonId1",
                table: "taxons");

            migrationBuilder.DropIndex(
                name: "IX_taxons_ParentTaxonId1",
                table: "taxons");

            migrationBuilder.DropIndex(
                name: "IX_Post_TaxonId1",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ParentTaxonId1",
                table: "taxons");

            migrationBuilder.DropColumn(
                name: "dwc_TaxonID",
                table: "taxons");

            migrationBuilder.DropColumn(
                name: "TaxonId1",
                table: "Post");

            migrationBuilder.AlterColumn<string>(
                name: "TaxonId",
                table: "taxons",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Proposal_TaxonId",
                table: "Post",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_taxons_ParentTaxonId",
                table: "taxons",
                column: "ParentTaxonId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_TaxonId",
                table: "Post",
                column: "TaxonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_taxons_TaxonId",
                table: "Post",
                column: "TaxonId",
                principalTable: "taxons",
                principalColumn: "TaxonId");

            migrationBuilder.AddForeignKey(
                name: "FK_taxons_taxons_ParentTaxonId",
                table: "taxons",
                column: "ParentTaxonId",
                principalTable: "taxons",
                principalColumn: "TaxonId");
        }
    }
}
