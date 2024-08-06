using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_schoolmgt_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsPrimaryResource",
            //    table: "tblOpMonthlyRoasterForSite",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.CreateTable(
                name: "tblParentsLogin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisteredPhone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegisteredEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InactiveOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentLogin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblParentsLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolAcademicBatches",
                columns: table => new
                {
                    AcademicYear = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcademicEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolAcademicBatches", x => x.AcademicYear);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolAcademicsSubects",
                columns: table => new
                {
                    SubCodes = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolAcademicsSubects", x => x.SubCodes);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolAcedemicClassGrade",
                columns: table => new
                {
                    GradeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolAcedemicClassGrade", x => x.GradeCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolGender",
                columns: table => new
                {
                    GenderCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenderName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolGender", x => x.GenderCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolLanguages",
                columns: table => new
                {
                    LangCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LangName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LangName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolLanguages", x => x.LangCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolNationality",
                columns: table => new
                {
                    NatCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NatName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolNationality", x => x.NatCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolPayTypes",
                columns: table => new
                {
                    PayCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GLAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowOtherBranchUse = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolPayTypes", x => x.PayCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolPETCategory",
                columns: table => new
                {
                    PETCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PETName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PETName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolPETCategory", x => x.PETCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolReligion",
                columns: table => new
                {
                    RegCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolReligion", x => x.RegCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolSectionsSection",
                columns: table => new
                {
                    SectionCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolSectionsSection", x => x.SectionCode);
                });

            migrationBuilder.CreateTable(
                name: "TblSysSchoolSemister",
                columns: table => new
                {
                    SemisterCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemisterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SemisterName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemisterStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemisterEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSysSchoolSemister", x => x.SemisterCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolStuLeaveType",
                columns: table => new
                {
                    LeaveCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaveName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxLeavePerReq = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolStuLeaveType", x => x.LeaveCode);
                });

            migrationBuilder.CreateTable(
                name: "tblWardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblWardDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblParentsLogin");

            migrationBuilder.DropTable(
                name: "tblSysSchoolAcademicBatches");

            migrationBuilder.DropTable(
                name: "tblSysSchoolAcademicsSubects");

            migrationBuilder.DropTable(
                name: "tblSysSchoolAcedemicClassGrade");

            migrationBuilder.DropTable(
                name: "tblSysSchoolGender");

            migrationBuilder.DropTable(
                name: "tblSysSchoolLanguages");

            migrationBuilder.DropTable(
                name: "tblSysSchoolNationality");

            migrationBuilder.DropTable(
                name: "tblSysSchoolPayTypes");

            migrationBuilder.DropTable(
                name: "tblSysSchoolPETCategory");

            migrationBuilder.DropTable(
                name: "tblSysSchoolReligion");

            migrationBuilder.DropTable(
                name: "tblSysSchoolSectionsSection");

            migrationBuilder.DropTable(
                name: "TblSysSchoolSemister");

            migrationBuilder.DropTable(
                name: "tblSysSchoolStuLeaveType");

            migrationBuilder.DropTable(
                name: "tblWardDetails");

            migrationBuilder.DropColumn(
                name: "IsPrimaryResource",
                table: "tblOpMonthlyRoasterForSite");
        }
    }
}
