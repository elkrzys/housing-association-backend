using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class FixUserIssuesIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_issues_author_id",
                table: "issues");

            migrationBuilder.CreateIndex(
                name: "ix_issues_author_id",
                table: "issues",
                column: "author_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_issues_author_id",
                table: "issues");

            migrationBuilder.CreateIndex(
                name: "ix_issues_author_id",
                table: "issues",
                column: "author_id",
                unique: true);
        }
    }
}
