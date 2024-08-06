using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_POHeader_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tblPopTrnPurchaseReturnHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ISGRN",
                table: "tblPopTrnPurchaseOrderHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGrn",
                table: "tblPopTrnPurchaseOrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "tblIMTransferTransactionHeader",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "tblIMTransferTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tblIMTransferTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "tblIMTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tblIMTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "tblIMReceiptsTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tblIMReceiptsTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "tblIMAdjustmentsTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "tblIMAdjustmentsTransactionHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GlId",
                table: "tblFinTrnAccountsLedger",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblPopTrnGRNDetails",
                columns: table => new
                {
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    VendCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompCode = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranVendorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemTracking = table.Column<short>(type: "smallint", nullable: false),
                    TranItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TranItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TranItemName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TranItemQty = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    TranItemUnitCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TranUOMFactor = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    TranItemCost = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    TranTotCost = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    DiscPer = table.Column<decimal>(type: "decimal(6,3)", nullable: false),
                    DiscAmt = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    ItemTax = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    ItemTaxPer = table.Column<decimal>(type: "decimal(6,3)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    POQtyReceived = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    POQtyReceiving = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    POQtyCancel = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    POLineCost1 = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    POLineCost2 = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    POOHCostPerItem = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    POLandedCost = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    POLandedCostPerItem = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    TranVoidStatus = table.Column<bool>(type: "bit", nullable: false),
                    TranPostStatus = table.Column<bool>(type: "bit", nullable: false),
                    ForeClosed = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    ReceivingQty = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    BalQty = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    ReceivedQty = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopTrnGRNDetails", x => x.TranNumber);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblErpInvItemMaster_TranItemCode",
                        column: x => x.TranItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblErpSysCompany_CompCode",
                        column: x => x.CompCode,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblInvDefUOM_TranItemUnitCode",
                        column: x => x.TranItemUnitCode,
                        principalTable: "tblInvDefUOM",
                        principalColumn: "UOMCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblPopTrnPurchaseOrderHeader_TranId",
                        column: x => x.TranId,
                        principalTable: "tblPopTrnPurchaseOrderHeader",
                        principalColumn: "TranNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblSndDefVendorMaster_TranVendorCode",
                        column: x => x.TranVendorCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNDetails_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblPopTrnGRNHeader",
                columns: table => new
                {
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VenCatCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Trantype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranDate = table.Column<DateTime>(type: "date", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "date", nullable: false),
                    CancelDate = table.Column<DateTime>(type: "date", nullable: false),
                    CompCode = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvRefNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RFQContractNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TAXId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxInclusive = table.Column<short>(type: "smallint", nullable: false),
                    PONotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TranBuyer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    TranCreateUserDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranCreateUser = table.Column<int>(type: "int", nullable: false),
                    TranLastEditDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranLastEditUser = table.Column<int>(type: "int", nullable: false),
                    TranPostStatus = table.Column<bool>(type: "bit", nullable: false),
                    TranPostDate = table.Column<DateTime>(type: "date", nullable: false),
                    TranpostUser = table.Column<int>(type: "int", nullable: false),
                    TranVoidStatus = table.Column<bool>(type: "bit", nullable: false),
                    TranVoidUser = table.Column<DateTime>(type: "date", nullable: false),
                    TranvoidDate = table.Column<int>(type: "int", nullable: false),
                    TranShipMode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TranCurrencyCode = table.Column<int>(type: "int", nullable: false),
                    ExRate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    TranTotalCost = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    TranDiscPer = table.Column<decimal>(type: "decimal(6,3)", nullable: false),
                    TranDiscAmount = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    OHCharges = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    POClosedDate = table.Column<DateTime>(type: "date", nullable: false),
                    ClosedBy = table.Column<int>(type: "int", nullable: false),
                    ForeClosed = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseRequestNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseOrderNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WHCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopTrnGRNHeader", x => x.TranNumber);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysCompany_CompCode",
                        column: x => x.CompCode,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysCurrencyCode_TranCurrencyCode",
                        column: x => x.TranCurrencyCode,
                        principalTable: "tblErpSysCurrencyCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysLogin_ClosedBy",
                        column: x => x.ClosedBy,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysLogin_TranCreateUser",
                        column: x => x.TranCreateUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysLogin_TranLastEditUser",
                        column: x => x.TranLastEditUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysLogin_TranpostUser",
                        column: x => x.TranpostUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblErpSysLogin_TranvoidDate",
                        column: x => x.TranvoidDate,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblPopDefVendorPOTermsCode_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "tblPopDefVendorPOTermsCode",
                        principalColumn: "POTermsCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblPopDefVendorShipment_TranShipMode",
                        column: x => x.TranShipMode,
                        principalTable: "tblPopDefVendorShipment",
                        principalColumn: "ShipmentCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnGRNHeader_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblIMTransferTransactionHeader_BranchCode",
                table: "tblIMTransferTransactionHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_BranchCode",
                table: "tblPopTrnGRNDetails",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_CompCode",
                table: "tblPopTrnGRNDetails",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_TranId",
                table: "tblPopTrnGRNDetails",
                column: "TranId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_TranItemCode",
                table: "tblPopTrnGRNDetails",
                column: "TranItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_TranItemUnitCode",
                table: "tblPopTrnGRNDetails",
                column: "TranItemUnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_TranVendorCode",
                table: "tblPopTrnGRNDetails",
                column: "TranVendorCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNDetails_VendCode",
                table: "tblPopTrnGRNDetails",
                column: "VendCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_BranchCode",
                table: "tblPopTrnGRNHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_ClosedBy",
                table: "tblPopTrnGRNHeader",
                column: "ClosedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_CompCode",
                table: "tblPopTrnGRNHeader",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_PaymentID",
                table: "tblPopTrnGRNHeader",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_TranCreateUser",
                table: "tblPopTrnGRNHeader",
                column: "TranCreateUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_TranCurrencyCode",
                table: "tblPopTrnGRNHeader",
                column: "TranCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_TranLastEditUser",
                table: "tblPopTrnGRNHeader",
                column: "TranLastEditUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_TranpostUser",
                table: "tblPopTrnGRNHeader",
                column: "TranpostUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_TranShipMode",
                table: "tblPopTrnGRNHeader",
                column: "TranShipMode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_TranvoidDate",
                table: "tblPopTrnGRNHeader",
                column: "TranvoidDate");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnGRNHeader_VendCode",
                table: "tblPopTrnGRNHeader",
                column: "VendCode");

            migrationBuilder.AddForeignKey(
                name: "FK_tblIMTransferTransactionHeader_tblErpSysCompanyBranches_BranchCode",
                table: "tblIMTransferTransactionHeader",
                column: "BranchCode",
                principalTable: "tblErpSysCompanyBranches",
                principalColumn: "BranchCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblIMTransferTransactionHeader_tblErpSysCompanyBranches_BranchCode",
                table: "tblIMTransferTransactionHeader");

            migrationBuilder.DropTable(
                name: "tblPopTrnGRNDetails");

            migrationBuilder.DropTable(
                name: "tblPopTrnGRNHeader");

            migrationBuilder.DropIndex(
                name: "IX_tblIMTransferTransactionHeader_BranchCode",
                table: "tblIMTransferTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tblPopTrnPurchaseReturnHeader");

            migrationBuilder.DropColumn(
                name: "ISGRN",
                table: "tblPopTrnPurchaseOrderHeader");

            migrationBuilder.DropColumn(
                name: "IsGrn",
                table: "tblPopTrnPurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "tblIMTransferTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "tblIMTransferTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tblIMTransferTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "tblIMTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tblIMTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "tblIMReceiptsTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tblIMReceiptsTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "tblIMAdjustmentsTransactionHeader");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "tblIMAdjustmentsTransactionHeader");

            migrationBuilder.DropColumn(
                name: "GlId",
                table: "tblFinTrnAccountsLedger");
        }
    }
}
