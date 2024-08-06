using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_cust_vend_creditlimit_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VendAvailCrLimit",
                table: "tblSndDefVendorMaster",
                type: "decimal(17,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CustAvailCrLimit",
                table: "tblSndDefCustomerMaster",
                type: "decimal(17,3)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "tblOpEmployeeLeaves",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        AttnDate = table.Column<DateTime>(type: "date", nullable: true),
            //        AL = table.Column<bool>(type: "bit", nullable: false),
            //        EL = table.Column<bool>(type: "bit", nullable: false),
            //        UL = table.Column<bool>(type: "bit", nullable: false),
            //        SL = table.Column<bool>(type: "bit", nullable: false),
            //        W = table.Column<bool>(type: "bit", nullable: false),
            //        STL = table.Column<bool>(type: "bit", nullable: false),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpEmployeeLeaves", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblOpEmployeeTransResign",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        AttnDate = table.Column<DateTime>(type: "date", nullable: true),
            //        TR = table.Column<bool>(type: "bit", nullable: false),
            //        R = table.Column<bool>(type: "bit", nullable: false),
            //        Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpEmployeeTransResign", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpEmployeeLeaves");

            migrationBuilder.DropTable(
                name: "tblOpEmployeeTransResign");

            migrationBuilder.DropColumn(
                name: "VendAvailCrLimit",
                table: "tblSndDefVendorMaster");

            migrationBuilder.DropColumn(
                name: "CustAvailCrLimit",
                table: "tblSndDefCustomerMaster");
        }
    }
}
