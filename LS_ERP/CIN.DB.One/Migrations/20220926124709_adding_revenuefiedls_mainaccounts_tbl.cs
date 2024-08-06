using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_revenuefiedls_mainaccounts_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ServiceType",
                table: "tblOprTrnApprovals",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceCode",
                table: "tblOprTrnApprovals",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "SiteCode",
            //    table: "tblOpPaymentTermsToProject",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "SiteCode",
            //    table: "tblOpContractTermsMapToProject",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FinIsRevenue",
                table: "tblFinDefMainAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FinIsRevenuetype",
                table: "tblFinDefMainAccounts",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Adjs",
                columns: table => new
                {
                    PositiveSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NegativeSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adjs", x => x.PositiveSum);
                });

            //migrationBuilder.CreateTable(
            //    name: "tblOpProjectSites",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ProjectNameEng = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        ProjectNameArb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        StartDate = table.Column<DateTime>(type: "date", nullable: true),
            //        EndDate = table.Column<DateTime>(type: "date", nullable: true),
            //        ActualEndDate = table.Column<DateTime>(type: "date", nullable: true),
            //        IsResourcesAssigned = table.Column<bool>(type: "bit", nullable: false),
            //        IsMaterialAssigned = table.Column<bool>(type: "bit", nullable: false),
            //        IsLogisticsAssigned = table.Column<bool>(type: "bit", nullable: false),
            //        IsShiftsAssigned = table.Column<bool>(type: "bit", nullable: false),
            //        IsExpenceOverheadsAssigned = table.Column<bool>(type: "bit", nullable: false),
            //        IsEstimationCompleted = table.Column<bool>(type: "bit", nullable: false),
            //        IsSkillSetsMapped = table.Column<bool>(type: "bit", nullable: false),
            //        IsConvertedToProposal = table.Column<bool>(type: "bit", nullable: false),
            //        IsConvrtedToContract = table.Column<bool>(type: "bit", nullable: false),
            //        BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsAdendum = table.Column<bool>(type: "bit", nullable: false),
            //        IsInProgress = table.Column<bool>(type: "bit", nullable: false),
            //        IsClosed = table.Column<bool>(type: "bit", nullable: false),
            //        IsSuspended = table.Column<bool>(type: "bit", nullable: false),
            //        IsCancelled = table.Column<bool>(type: "bit", nullable: false),
            //        IsInActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpProjectSites", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblOpPvOpenCloseReq",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        IsSuspendReq = table.Column<bool>(type: "bit", nullable: false),
            //        IsCancelReq = table.Column<bool>(type: "bit", nullable: false),
            //        IsCloseReq = table.Column<bool>(type: "bit", nullable: false),
            //        IsReOpenReq = table.Column<bool>(type: "bit", nullable: false),
            //        IsRevokeSuspReq = table.Column<bool>(type: "bit", nullable: false),
            //        IsExtendProjReq = table.Column<bool>(type: "bit", nullable: false),
            //        EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
            //        ExtensionDate = table.Column<DateTime>(type: "date", nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        ApprovedBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpPvOpenCloseReq", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adjs");

            migrationBuilder.DropTable(
                name: "tblOpProjectSites");

            migrationBuilder.DropTable(
                name: "tblOpPvOpenCloseReq");

            migrationBuilder.DropColumn(
                name: "SiteCode",
                table: "tblOpPaymentTermsToProject");

            migrationBuilder.DropColumn(
                name: "SiteCode",
                table: "tblOpContractTermsMapToProject");

            migrationBuilder.DropColumn(
                name: "FinIsRevenue",
                table: "tblFinDefMainAccounts");

            migrationBuilder.DropColumn(
                name: "FinIsRevenuetype",
                table: "tblFinDefMainAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceType",
                table: "tblOprTrnApprovals",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceCode",
                table: "tblOprTrnApprovals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
