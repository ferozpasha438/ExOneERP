using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_sitecode_sysLogintbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasGoeSiteId",
                table: "tblErpSysLogin");

            migrationBuilder.AddColumn<string>(
                name: "SiteCode",
                table: "tblErpSysLogin",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblInvDefTracking",
                columns: table => new
                {
                    TrCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TrName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefTracking", x => x.TrCode);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefType",
                columns: table => new
                {
                    TypeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefType", x => x.TypeCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblInvDefTracking");

            migrationBuilder.DropTable(
                name: "tblInvDefType");

            migrationBuilder.DropColumn(
                name: "SiteCode",
                table: "tblErpSysLogin");

            migrationBuilder.AddColumn<bool>(
                name: "HasGoeSiteId",
                table: "tblErpSysLogin",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
