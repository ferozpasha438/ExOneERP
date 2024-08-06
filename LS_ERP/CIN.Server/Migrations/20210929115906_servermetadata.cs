using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CIN.Server.Migrations
{
    public partial class servermetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CINServerMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CINNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ValidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APIEndpoint = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DBConnectionString = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CINServerMetaData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CINServerMetaData");
        }
    }
}
