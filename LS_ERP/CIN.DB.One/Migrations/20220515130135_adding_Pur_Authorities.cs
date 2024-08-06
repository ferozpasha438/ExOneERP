using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_Pur_Authorities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRM_DEF_EmployeeShiftMaster");

            migrationBuilder.DropTable(
                name: "HRM_TRAN_Employee");

            migrationBuilder.DropTable(
                name: "tblOpShiftSiteMap");

            //migrationBuilder.AddColumn<string>(
            //    name: "EmployeeNumber",
            //    table: "tblOpEmployeeToResourceMap",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "isPrimarySite",
            //    table: "tblOpEmployeeToResourceMap",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "EmployeeName",
            //    table: "tblOpEmployeesToProjectSite",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "isLiabilityAndInsurance",
            //    table: "tblOpContractTermsMapToProject",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "isTerminationClause",
            //    table: "tblOpContractTermsMapToProject",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "tblOpEmployeeAttendance",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        AttnDate = table.Column<DateTime>(type: "date", nullable: true),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ShiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        InTime = table.Column<TimeSpan>(type: "time", nullable: false),
            //        OutTime = table.Column<TimeSpan>(type: "time", nullable: false),
            //        ShiftNumber = table.Column<short>(type: "smallint", nullable: true),
            //        isDefaultEmployee = table.Column<bool>(type: "bit", nullable: false),
            //        isPrimarySite = table.Column<bool>(type: "bit", nullable: false),
            //        isDefShiftOff = table.Column<bool>(type: "bit", nullable: false),
            //        isPosted = table.Column<bool>(type: "bit", nullable: false),
            //        Attendance = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
            //        AltEmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        AltShiftCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        RefIdForAlt = table.Column<long>(type: "bigint", nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpEmployeeAttendance", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "tblPurAuthorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppAuth = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppLevel = table.Column<short>(type: "smallint", nullable: false),
                    PurchaseRequest = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseOrder = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseReturn = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPurAuthorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPurTrnApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ServiceCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppAuth = table.Column<int>(type: "int", nullable: false),
                    AppRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPurTrnApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblPurTrnApprovals_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblPurTrnApprovals_BranchCode",
                table: "tblPurTrnApprovals",
                column: "BranchCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpEmployeeAttendance");

            migrationBuilder.DropTable(
                name: "tblPurAuthorities");

            migrationBuilder.DropTable(
                name: "tblPurTrnApprovals");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "tblOpEmployeeToResourceMap");

            migrationBuilder.DropColumn(
                name: "isPrimarySite",
                table: "tblOpEmployeeToResourceMap");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "tblOpEmployeesToProjectSite");

            migrationBuilder.DropColumn(
                name: "isLiabilityAndInsurance",
                table: "tblOpContractTermsMapToProject");

            migrationBuilder.DropColumn(
                name: "isTerminationClause",
                table: "tblOpContractTermsMapToProject");

            migrationBuilder.CreateTable(
                name: "HRM_DEF_EmployeeShiftMaster",
                columns: table => new
                {
                    ShiftId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreakTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InGrace = table.Column<TimeSpan>(type: "time", nullable: true),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsOff = table.Column<bool>(type: "bit", nullable: false),
                    NetWorkingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutGrace = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ShiftCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShiftName_AR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShiftName_EN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_DEF_EmployeeShiftMaster", x => x.ShiftId);
                });

            migrationBuilder.CreateTable(
                name: "HRM_TRAN_Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_TRAN_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "tblOpShiftSiteMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOpShiftSiteMap", x => x.Id);
                });
        }
    }
}
