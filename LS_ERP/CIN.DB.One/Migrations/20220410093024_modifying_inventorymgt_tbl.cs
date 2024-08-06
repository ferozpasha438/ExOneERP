using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class modifying_inventorymgt_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderNO",
                table: "tblPopTrnPurchaseOrderHeader",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseRequestNO",
                table: "tblPopTrnPurchaseOrderHeader",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblIMTransactionHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIMTransactionHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPopTrnPurchaseReturnHeader",
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
                    PurchaseReturnNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopTrnPurchaseReturnHeader", x => x.TranNumber);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysCompany_CompCode",
                        column: x => x.CompCode,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysCurrencyCode_TranCurrencyCode",
                        column: x => x.TranCurrencyCode,
                        principalTable: "tblErpSysCurrencyCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysLogin_ClosedBy",
                        column: x => x.ClosedBy,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysLogin_TranCreateUser",
                        column: x => x.TranCreateUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysLogin_TranLastEditUser",
                        column: x => x.TranLastEditUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysLogin_TranpostUser",
                        column: x => x.TranpostUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblErpSysLogin_TranvoidDate",
                        column: x => x.TranvoidDate,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblPopDefVendorPOTermsCode_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "tblPopDefVendorPOTermsCode",
                        principalColumn: "POTermsCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblPopDefVendorShipment_TranShipMode",
                        column: x => x.TranShipMode,
                        principalTable: "tblPopDefVendorShipment",
                        principalColumn: "ShipmentCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnHeader_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblTblIMTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SNo = table.Column<int>(type: "int", nullable: false),
                    TranNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_tblTblIMTransactionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPopTrnPurchaseReturnDetails",
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopTrnPurchaseReturnDetails", x => x.TranNumber);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblErpInvItemMaster_TranItemCode",
                        column: x => x.TranItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblErpSysCompany_CompCode",
                        column: x => x.CompCode,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblInvDefUOM_TranItemUnitCode",
                        column: x => x.TranItemUnitCode,
                        principalTable: "tblInvDefUOM",
                        principalColumn: "UOMCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblPopTrnPurchaseReturnHeader_TranId",
                        column: x => x.TranId,
                        principalTable: "tblPopTrnPurchaseReturnHeader",
                        principalColumn: "TranNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblSndDefVendorMaster_TranVendorCode",
                        column: x => x.TranVendorCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseReturnDetails_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_BranchCode",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_CompCode",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_TranId",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "TranId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_TranItemCode",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "TranItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_TranItemUnitCode",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "TranItemUnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_TranVendorCode",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "TranVendorCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnDetails_VendCode",
                table: "tblPopTrnPurchaseReturnDetails",
                column: "VendCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_BranchCode",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_ClosedBy",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "ClosedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_CompCode",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_PaymentID",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_TranCreateUser",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "TranCreateUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_TranCurrencyCode",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "TranCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_TranLastEditUser",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "TranLastEditUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_TranpostUser",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "TranpostUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_TranShipMode",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "TranShipMode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_TranvoidDate",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "TranvoidDate");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseReturnHeader_VendCode",
                table: "tblPopTrnPurchaseReturnHeader",
                column: "VendCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblIMTransactionHeader");

            migrationBuilder.DropTable(
                name: "tblPopTrnPurchaseReturnDetails");

            migrationBuilder.DropTable(
                name: "tblTblIMTransactionDetails");

            migrationBuilder.DropTable(
                name: "tblPopTrnPurchaseReturnHeader");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderNO",
                table: "tblPopTrnPurchaseOrderHeader");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestNO",
                table: "tblPopTrnPurchaseOrderHeader");
        }
    }
}
