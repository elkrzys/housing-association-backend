using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class AddDocumentRemovesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "days_to_expire",
                table: "documents");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "removes",
                table: "documents",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "removes",
                table: "documents");

            migrationBuilder.AddColumn<int>(
                name: "days_to_expire",
                table: "documents",
                type: "integer",
                nullable: true);
        }
    }
}
