using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class RemoveUnusedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_announcements_type_Enum",
                table: "announcements");

            migrationBuilder.DropColumn(
                name: "is_fully_owned",
                table: "locals");

            migrationBuilder.DropColumn(
                name: "type",
                table: "announcements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_fully_owned",
                table: "locals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "announcements",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_announcements_type_Enum",
                table: "announcements",
                sql: "type IN ('Issue', 'Announcement', 'Alert')");
        }
    }
}
