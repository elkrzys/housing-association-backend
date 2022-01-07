using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class RemoveLocalOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locals_owners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locals_owners",
                columns: table => new
                {
                    owned_locals_id = table.Column<int>(type: "integer", nullable: false),
                    owners_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locals_owners", x => new { x.owned_locals_id, x.owners_id });
                    table.ForeignKey(
                        name: "fk_locals_owners_locals_owned_locals_id",
                        column: x => x.owned_locals_id,
                        principalTable: "locals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_locals_owners_users_owners_id",
                        column: x => x.owners_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_locals_owners_owners_id",
                table: "locals_owners",
                column: "owners_id");
        }
    }
}
