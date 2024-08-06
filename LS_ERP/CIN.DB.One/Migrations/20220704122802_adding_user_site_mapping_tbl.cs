using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_user_site_mapping_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SiteGeoLatitudeMeter",
                table: "tblSndDefSiteMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SiteGeoLongitudeMeter",
                table: "tblSndDefSiteMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "tblErpSysUserSiteMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpSysUserSiteMapping", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblErpSysUserSiteMapping");

            migrationBuilder.DropColumn(
                name: "SiteGeoLatitudeMeter",
                table: "tblSndDefSiteMaster");

            migrationBuilder.DropColumn(
                name: "SiteGeoLongitudeMeter",
                table: "tblSndDefSiteMaster");
        }
    }
}
