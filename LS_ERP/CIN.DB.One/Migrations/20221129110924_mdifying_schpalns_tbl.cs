using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class mdifying_schpalns_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblLessonPlanHeader",
                columns: table => new
                {
                    LessonPlanCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumOfLessons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOfDays = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLessonPlanHeader", x => x.LessonPlanCode);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolNews",
                columns: table => new
                {
                    NewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Headlines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewTumbnailImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolNews", x => x.NewId);
                });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolNewsMediaLib",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewId = table.Column<int>(type: "int", nullable: false),
                    Mediapath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolNewsMediaLib", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSysSchoolNewsMediaLib_tblSysSchoolNews_NewId",
                        column: x => x.NewId,
                        principalTable: "tblSysSchoolNews",
                        principalColumn: "NewId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSysSchoolNewsMediaLib_NewId",
                table: "tblSysSchoolNewsMediaLib",
                column: "NewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblLessonPlanHeader");

            migrationBuilder.DropTable(
                name: "tblSysSchoolNewsMediaLib");

            migrationBuilder.DropTable(
                name: "tblSysSchoolNews");
        }
    }
}
