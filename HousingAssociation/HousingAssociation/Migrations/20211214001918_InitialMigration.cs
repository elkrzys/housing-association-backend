using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HousingAssociation.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    district = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.CheckConstraint("CK_users_role_Enum", "role IN ('Admin', 'Worker', 'Resident')");
                });

            migrationBuilder.CreateTable(
                name: "buildings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buildings", x => x.id);
                    table.CheckConstraint("CK_buildings_type_Enum", "type IN ('Block', 'House')");
                    table.ForeignKey(
                        name: "fk_buildings_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_cancelled_or_expired = table.Column<bool>(type: "boolean", nullable: false),
                    previous_announcement_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_announcements", x => x.id);
                    table.CheckConstraint("CK_announcements_type_Enum", "type IN ('Issue', 'Announcement', 'Alert')");
                    table.ForeignKey(
                        name: "fk_announcements_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credentials", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_credentials_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    filepath = table.Column<string>(type: "text", nullable: false),
                    md5 = table.Column<string>(type: "text", nullable: false),
                    days_to_expire = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_documents_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<string>(type: "text", nullable: true),
                    expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    revoked = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    replaced_by_token = table.Column<string>(type: "text", nullable: true),
                    reason_revoked = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "locals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: true),
                    area = table.Column<float>(type: "real", nullable: true),
                    building_id = table.Column<int>(type: "integer", nullable: false),
                    is_fully_owned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locals", x => x.id);
                    table.ForeignKey(
                        name: "fk_locals_buildings_building_id",
                        column: x => x.building_id,
                        principalTable: "buildings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "announcements_buildings",
                columns: table => new
                {
                    announcements_id = table.Column<int>(type: "integer", nullable: false),
                    target_buildings_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_announcements_buildings", x => new { x.announcements_id, x.target_buildings_id });
                    table.ForeignKey(
                        name: "fk_announcements_buildings_announcements_announcements_id",
                        column: x => x.announcements_id,
                        principalTable: "announcements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_announcements_buildings_buildings_target_buildings_id",
                        column: x => x.target_buildings_id,
                        principalTable: "buildings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_documents",
                columns: table => new
                {
                    documents_id = table.Column<int>(type: "integer", nullable: false),
                    receivers_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_documents", x => new { x.documents_id, x.receivers_id });
                    table.ForeignKey(
                        name: "fk_users_documents_documents_documents_id",
                        column: x => x.documents_id,
                        principalTable: "documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_documents_users_receivers_id",
                        column: x => x.receivers_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "issues",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    source_local_id = table.Column<int>(type: "integer", nullable: true),
                    source_building_id = table.Column<int>(type: "integer", nullable: false),
                    is_resolved = table.Column<bool>(type: "boolean", nullable: false),
                    is_cancelled = table.Column<bool>(type: "boolean", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issues", x => x.id);
                    table.ForeignKey(
                        name: "fk_issues_buildings_source_building_id",
                        column: x => x.source_building_id,
                        principalTable: "buildings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_issues_locals_source_local_id",
                        column: x => x.source_local_id,
                        principalTable: "locals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_issues_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "locals_residents",
                columns: table => new
                {
                    resided_locals_id = table.Column<int>(type: "integer", nullable: false),
                    residents_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locals_residents", x => new { x.resided_locals_id, x.residents_id });
                    table.ForeignKey(
                        name: "fk_locals_residents_locals_resided_locals_id",
                        column: x => x.resided_locals_id,
                        principalTable: "locals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_locals_residents_users_residents_id",
                        column: x => x.residents_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_announcements_author_id",
                table: "announcements",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_announcements_buildings_target_buildings_id",
                table: "announcements_buildings",
                column: "target_buildings_id");

            migrationBuilder.CreateIndex(
                name: "ix_buildings_address_id",
                table: "buildings",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_author_id",
                table: "documents",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_issues_author_id",
                table: "issues",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_issues_source_building_id",
                table: "issues",
                column: "source_building_id");

            migrationBuilder.CreateIndex(
                name: "ix_issues_source_local_id",
                table: "issues",
                column: "source_local_id");

            migrationBuilder.CreateIndex(
                name: "ix_locals_building_id",
                table: "locals",
                column: "building_id");

            migrationBuilder.CreateIndex(
                name: "ix_locals_owners_owners_id",
                table: "locals_owners",
                column: "owners_id");

            migrationBuilder.CreateIndex(
                name: "ix_locals_residents_residents_id",
                table: "locals_residents",
                column: "residents_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_documents_receivers_id",
                table: "users_documents",
                column: "receivers_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcements_buildings");

            migrationBuilder.DropTable(
                name: "credentials");

            migrationBuilder.DropTable(
                name: "issues");

            migrationBuilder.DropTable(
                name: "locals_owners");

            migrationBuilder.DropTable(
                name: "locals_residents");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "users_documents");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "locals");

            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "buildings");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
