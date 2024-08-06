using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invtmgtment_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblIMTransactionHeader",
                table: "tblIMTransactionHeader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblTblIMTransactionDetails",
                table: "tblTblIMTransactionDetails");

            migrationBuilder.RenameTable(
                name: "tblTblIMTransactionDetails",
                newName: "tblIMTransactionDetails");

            migrationBuilder.AlterColumn<string>(
                name: "TranNumber",
                table: "tblIMTransactionHeader",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SNo",
                table: "tblIMTransactionDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblIMTransactionHeader",
                table: "tblIMTransactionHeader",
                column: "TranNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblIMTransactionDetails",
                table: "tblIMTransactionDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tblIMAdjustmentsTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TranBarcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TranItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemQty = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TranItemUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranUOMFactor = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    TranItemCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    ItemAttribute1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ItemAttribute2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVADJAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMAdjustmentsTransactionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblIMAdjustmentsTransactionHeader",
                columns: table => new
                {
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranDocNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranTotalCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotItems = table.Column<int>(type: "int", nullable: false),
                    TranCreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranCreateUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLastEditDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLastEditUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLockStat = table.Column<short>(type: "smallint", nullable: false),
                    TranLockUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranPostStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranPostDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranpostUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranVoidStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranVoidUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranvoidDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAdjAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JVNum = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMAdjustmentsTransactionHeader", x => x.TranNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblIMReceiptsTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TranBarcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TranItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemQty = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TranItemUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranUOMFactor = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    TranItemCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    ItemAttribute1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ItemAttribute2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVADJAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMReceiptsTransactionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblIMReceiptsTransactionHeader",
                columns: table => new
                {
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranDocNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranTotalCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotItems = table.Column<int>(type: "int", nullable: false),
                    TranCreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranCreateUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLastEditDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLastEditUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLockStat = table.Column<short>(type: "smallint", nullable: false),
                    TranLockUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranPostStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranPostDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranpostUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranVoidStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranVoidUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranvoidDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAdjAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JVNum = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMReceiptsTransactionHeader", x => x.TranNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblIMStockReconciliationTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TranBarcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TranItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemQty = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TranItemUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranUOMFactor = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    TranItemCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    ItemAttribute1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ItemAttribute2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVADJAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMStockReconciliationTransactionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblIMStockReconciliationTransactionHeader",
                columns: table => new
                {
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranDocNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranTotalCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotItems = table.Column<int>(type: "int", nullable: false),
                    TranCreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranCreateUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLastEditDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLastEditUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLockStat = table.Column<short>(type: "smallint", nullable: false),
                    TranLockUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranPostStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranPostDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranpostUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranVoidStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranVoidUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranvoidDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAdjAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JVNum = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMStockReconciliationTransactionHeader", x => x.TranNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblIMTransferTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TranBarcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TranItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TranItemQty = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    TranItemUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranUOMFactor = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    TranItemCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    ItemAttribute1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ItemAttribute2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVADJAcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMTransferTransactionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblIMTransferTransactionHeader",
                columns: table => new
                {
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranToLocation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranDocNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranTotalCost = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TranTotItems = table.Column<int>(type: "int", nullable: false),
                    TranCreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranCreateUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLastEditDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLastEditUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranLockStat = table.Column<short>(type: "smallint", nullable: false),
                    TranLockUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranPostStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranPostDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranpostUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranVoidStatus = table.Column<short>(type: "smallint", nullable: false),
                    TranVoidUser = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    TranvoidDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TranInvAdjAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JVNum = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMTransferTransactionHeader", x => x.TranNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblIMAdjustmentsTransactionDetails");

            migrationBuilder.DropTable(
                name: "tblIMAdjustmentsTransactionHeader");

            migrationBuilder.DropTable(
                name: "tblIMReceiptsTransactionDetails");

            migrationBuilder.DropTable(
                name: "tblIMReceiptsTransactionHeader");

            migrationBuilder.DropTable(
                name: "tblIMStockReconciliationTransactionDetails");

            migrationBuilder.DropTable(
                name: "tblIMStockReconciliationTransactionHeader");

            migrationBuilder.DropTable(
                name: "tblIMTransferTransactionDetails");

            migrationBuilder.DropTable(
                name: "tblIMTransferTransactionHeader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblIMTransactionHeader",
                table: "tblIMTransactionHeader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblIMTransactionDetails",
                table: "tblIMTransactionDetails");

            migrationBuilder.RenameTable(
                name: "tblIMTransactionDetails",
                newName: "tblTblIMTransactionDetails");

            migrationBuilder.AlterColumn<string>(
                name: "TranNumber",
                table: "tblIMTransactionHeader",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "SNo",
                table: "tblTblIMTransactionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblIMTransactionHeader",
                table: "tblIMTransactionHeader",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblTblIMTransactionDetails",
                table: "tblTblIMTransactionDetails",
                column: "Id");
        }
    }
}
