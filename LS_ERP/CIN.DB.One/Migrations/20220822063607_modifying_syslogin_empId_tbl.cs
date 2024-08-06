using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_syslogin_empId_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeID",
                table: "tblErpSysUserSiteMapping",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblOpPvAddResource",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddResReqHeadId = table.Column<long>(type: "bigint", nullable: false),
                    Qty = table.Column<short>(type: "smallint", nullable: false),
                    SkillsetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PricePerUnit = table.Column<decimal>(type: "decimal(17,3)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpPvAddResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblOpPvAddResourceReqHead",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsEmpMapped = table.Column<bool>(type: "bit", nullable: false),
                    IsMerged = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpPvAddResourceReqHead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolFeeTerms",
                columns: table => new
                {
                    TermCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeeDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolFeeTerms", x => x.TermCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolFeeType",
                columns: table => new
                {
                    FeeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeesName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDiscountable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDiscPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxApplicable = table.Column<bool>(type: "bit", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeGLAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeTaxAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolFeeType", x => x.FeeCode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpPvAddResource");

            migrationBuilder.DropTable(
                name: "tblOpPvAddResourceReqHead");

            migrationBuilder.DropTable(
                name: "tblSysSchoolFeeTerms");

            migrationBuilder.DropTable(
                name: "tblSysSchoolFeeType");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "tblErpSysUserSiteMapping");
        }
    }
}
