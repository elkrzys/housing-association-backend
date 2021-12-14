using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class SetNullLocalInIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_locals_source_local_id",
                table: "issues");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_locals_source_local_id",
                table: "issues",
                column: "source_local_id",
                principalTable: "locals",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_locals_source_local_id",
                table: "issues");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_locals_source_local_id",
                table: "issues",
                column: "source_local_id",
                principalTable: "locals",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
