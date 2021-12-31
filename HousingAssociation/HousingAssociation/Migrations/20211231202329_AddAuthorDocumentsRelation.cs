using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class AddAuthorDocumentsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_documents_documents_documents_id",
                table: "users_documents");

            migrationBuilder.DropIndex(
                name: "ix_documents_author_id",
                table: "documents");

            migrationBuilder.RenameColumn(
                name: "documents_id",
                table: "users_documents",
                newName: "received_documents_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_author_id",
                table: "documents",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_documents_documents_received_documents_id",
                table: "users_documents",
                column: "received_documents_id",
                principalTable: "documents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_documents_documents_received_documents_id",
                table: "users_documents");

            migrationBuilder.DropIndex(
                name: "ix_documents_author_id",
                table: "documents");

            migrationBuilder.RenameColumn(
                name: "received_documents_id",
                table: "users_documents",
                newName: "documents_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_author_id",
                table: "documents",
                column: "author_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_users_documents_documents_documents_id",
                table: "users_documents",
                column: "documents_id",
                principalTable: "documents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
