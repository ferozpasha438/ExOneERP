using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class addedAssignEmployeestoProjectSite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "MonthEndDate",
            //    table: "tblOpPaymentTermsToProject");

            //migrationBuilder.DropColumn(
            //    name: "MonthStartDate",
            //    table: "tblOpPaymentTermsToProject");

            //migrationBuilder.DropColumn(
            //    name: "CanEditSurveyForm",
            //    table: "tblOpAuthorities");

            migrationBuilder.CreateTable(
                name: "tblOpEmployeesToProjectSite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpEmployeesToProjectSite", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpEmployeesToProjectSite");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "MonthEndDate",
            //    table: "tblOpPaymentTermsToProject",
            //    type: "date",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "MonthStartDate",
            //    table: "tblOpPaymentTermsToProject",
            //    type: "datetime2",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "CanEditSurveyForm",
            //    table: "tblOpAuthorities",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);
        }
    }
}
