using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_scsectionmapping_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "ApprovedBy",
            //    table: "tblOpPvAddResourceReqHead",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Qty",
                table: "tblOpPvAddResource",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "FromDate",
            //    table: "tblOpPvAddResource",
            //    type: "date",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "ToDate",
            //    table: "tblOpPvAddResource",
            //    type: "date",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "CanApprovePvReq",
            //    table: "tblOpAuthorities",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "CanEditPvReq",
            //    table: "tblOpAuthorities",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.CreateTable(
            //    name: "tblOpPvAddResourceEmployeeToResourceMap",
            //    columns: table => new
            //    {
            //        MapId = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PvAddResReqId = table.Column<long>(type: "bigint", nullable: false),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SkillSet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        DefShift = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        OffDay = table.Column<short>(type: "smallint", nullable: false),
            //        FromDate = table.Column<DateTime>(type: "date", nullable: true),
            //        ToDate = table.Column<DateTime>(type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpPvAddResourceEmployeeToResourceMap", x => x.MapId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblOpPvRemoveResourceReq",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        FromDate = table.Column<DateTime>(type: "date", nullable: true),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        IsMerged = table.Column<bool>(type: "bit", nullable: false),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        ApprovedBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpPvRemoveResourceReq", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblOpPvReplaceResourceReq",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ResignedEmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        ReplacedEmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        FromDate = table.Column<DateTime>(type: "date", nullable: true),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        IsMerged = table.Column<bool>(type: "bit", nullable: false),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        ApprovedBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpPvReplaceResourceReq", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tblOpPvTransferResourceReq",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SrcCustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SrcSiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        SrcProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        DestCustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        DestSiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        DestProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        FromDate = table.Column<DateTime>(type: "date", nullable: true),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        IsMerged = table.Column<bool>(type: "bit", nullable: false),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        ApprovedBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblOpPvTransferResourceReq", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "tblSysSchoolGradeSectionMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxStrength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinStrength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvgStrength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSysSchoolGradeSectionMapping", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOpPvAddResourceEmployeeToResourceMap");

            migrationBuilder.DropTable(
                name: "tblOpPvRemoveResourceReq");

            migrationBuilder.DropTable(
                name: "tblOpPvReplaceResourceReq");

            migrationBuilder.DropTable(
                name: "tblOpPvTransferResourceReq");

            migrationBuilder.DropTable(
                name: "tblSysSchoolGradeSectionMapping");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "tblOpPvAddResourceReqHead");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "tblOpPvAddResource");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "tblOpPvAddResource");

            migrationBuilder.DropColumn(
                name: "CanApprovePvReq",
                table: "tblOpAuthorities");

            migrationBuilder.DropColumn(
                name: "CanEditPvReq",
                table: "tblOpAuthorities");

            migrationBuilder.AlterColumn<short>(
                name: "Qty",
                table: "tblOpPvAddResource",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
