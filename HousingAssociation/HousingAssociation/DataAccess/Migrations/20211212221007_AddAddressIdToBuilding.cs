using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.DataAccess.Migrations
{
    public partial class AddAddressIdToBuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_buildings_addresses_address_id",
                table: "buildings");

            migrationBuilder.AddColumn<string>(
                name: "md5",
                table: "documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "address_id",
                table: "buildings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "previous_announcement_id",
                table: "announcements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_buildings_addresses_address_id",
                table: "buildings",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_buildings_addresses_address_id",
                table: "buildings");

            migrationBuilder.DropColumn(
                name: "md5",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "previous_announcement_id",
                table: "announcements");

            migrationBuilder.AlterColumn<int>(
                name: "address_id",
                table: "buildings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_buildings_addresses_address_id",
                table: "buildings",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
