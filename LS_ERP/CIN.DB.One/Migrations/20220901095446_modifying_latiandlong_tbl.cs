using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_latiandlong_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SiteGeoLongitude",
                table: "tblSndDefSiteMaster",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SiteGeoLatitude",
                table: "tblSndDefSiteMaster",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SiteGeoGain",
                table: "tblSndDefSiteMaster",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)");

            migrationBuilder.CreateTable(
                name: "tblSysSchoolBranches",
                columns: table => new
                {
                    BranchCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekOff1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeekOff2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeoLong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolBranches", x => x.BranchCode);
                });

            migrationBuilder.CreateTable(
                name: "TblWebStudentRegistration",
                columns: table => new
                {
                    FullName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishFluencyLevel = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsyourchildPottytrained = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblWebStudentRegistration", x => x.FullName);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolFeeStructureHeader",
                columns: table => new
                {
                    FeeStructCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FeeStructName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeStructName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplyLateFee = table.Column<bool>(type: "bit", nullable: false),
                    LateFeeCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ActualFeePayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolFeeStructureHeader", x => x.FeeStructCode);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolFeeStructureHeader_tblSysSchoolAcedemicClassGrade_GradeCode",
                        column: x => x.GradeCode,
                        principalTable: "tblSysSchoolAcedemicClassGrade",
                        principalColumn: "GradeCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolFeeStructureHeader_tblSysSchoolBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblSysSchoolBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolFeeStructureHeader_tblSysSchoolFeeType_LateFeeCode",
                        column: x => x.LateFeeCode,
                        principalTable: "tblSysSchoolFeeType",
                        principalColumn: "FeeCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolFeeStructureDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeStructCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TermCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FeeCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDiscPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolFeeStructureDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolFeeStructureDetails_tblSysSchoolFeeStructureHeader_FeeStructCode",
                        column: x => x.FeeStructCode,
                        principalTable: "tblSysSchoolFeeStructureHeader",
                        principalColumn: "FeeStructCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolFeeStructureDetails_tblSysSchoolFeeTerms_TermCode",
                        column: x => x.TermCode,
                        principalTable: "tblSysSchoolFeeTerms",
                        principalColumn: "TermCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolFeeStructureDetails_tblSysSchoolFeeType_FeeCode",
                        column: x => x.FeeCode,
                        principalTable: "tblSysSchoolFeeType",
                        principalColumn: "FeeCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolFeeStructureDetails_FeeCode",
                table: "tblSysSchoolFeeStructureDetails",
                column: "FeeCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolFeeStructureDetails_FeeStructCode",
                table: "tblSysSchoolFeeStructureDetails",
                column: "FeeStructCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolFeeStructureDetails_TermCode",
                table: "tblSysSchoolFeeStructureDetails",
                column: "TermCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolFeeStructureHeader_BranchCode",
                table: "tblSysSchoolFeeStructureHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolFeeStructureHeader_GradeCode",
                table: "tblSysSchoolFeeStructureHeader",
                column: "GradeCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolFeeStructureHeader_LateFeeCode",
                table: "tblSysSchoolFeeStructureHeader",
                column: "LateFeeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSysSchoolFeeStructureDetails");

            migrationBuilder.DropTable(
                name: "TblWebStudentRegistration");

            migrationBuilder.DropTable(
                name: "tblSysSchoolFeeStructureHeader");

            migrationBuilder.DropTable(
                name: "tblSysSchoolBranches");

            migrationBuilder.AlterColumn<decimal>(
                name: "SiteGeoLongitude",
                table: "tblSndDefSiteMaster",
                type: "decimal(7,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SiteGeoLatitude",
                table: "tblSndDefSiteMaster",
                type: "decimal(7,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SiteGeoGain",
                table: "tblSndDefSiteMaster",
                type: "decimal(7,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");
        }
    }
}
