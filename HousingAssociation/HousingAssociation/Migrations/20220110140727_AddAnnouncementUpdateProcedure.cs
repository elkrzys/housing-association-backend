using Microsoft.EntityFrameworkCore.Migrations;

namespace HousingAssociation.Migrations
{
    public partial class AddAnnouncementUpdateProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"CREATE OR REPLACE FUNCTION update_expired_announcements() RETURNS void AS $$
            BEGIN
                UPDATE announcements 
                SET expired = NOW() 
                WHERE cancelled is NULL AND expired is NULL AND expiration_date <= NOW();
            END;
                $$ LANGUAGE plpgsql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION update_expired_announcements");
        }
    }
}
