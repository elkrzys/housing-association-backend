using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class RemoveBuildingFromIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_buildings_source_building_id",
                table: "issues");

            migrationBuilder.DropIndex(
                name: "ix_issues_source_building_id",
                table: "issues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_issues_source_building_id",
                table: "issues",
                column: "source_building_id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_buildings_source_building_id",
                table: "issues",
                column: "source_building_id",
                principalTable: "buildings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
