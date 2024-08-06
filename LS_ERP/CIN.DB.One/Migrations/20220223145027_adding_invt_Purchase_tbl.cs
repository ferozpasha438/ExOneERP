using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CIN.DB.One.Migrations
{
    public partial class adding_invt_Purchase_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblInvDefSubCategory",
                table: "tblInvDefSubCategory");

            migrationBuilder.AlterColumn<string>(
                name: "SubCatKey",
                table: "tblInvDefSubCategory",
                type: "nvarchar(41)",
                maxLength: 41,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(41)",
                oldMaxLength: 41);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblInvDefSubCategory",
                table: "tblInvDefSubCategory",
                column: "ItemSubCatCode");

            migrationBuilder.CreateTable(
                name: "tblErpInvItemMaster",
                columns: table => new
                {
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HSNCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ItemDescriptionAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ShortNameAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ItemCat = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemSubCat = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemSubClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemBaseUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ItemAvgCost = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ItemStandardCost = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ItemPrimeVendor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemStandardPrice1 = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemStandardPrice2 = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemStandardPrice3 = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemTracking = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemWeight = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemTaxCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AllowPriceOverride = table.Column<bool>(type: "bit", nullable: false),
                    AllowDiscounts = table.Column<bool>(type: "bit", nullable: false),
                    AllowQuantityOverride = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpInvItemMaster", x => x.ItemCode);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemMaster_tblErpSysSystemTaxes_ItemTaxCode",
                        column: x => x.ItemTaxCode,
                        principalTable: "tblErpSysSystemTaxes",
                        principalColumn: "TaxCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemMaster_tblInvDefCategory_ItemCat",
                        column: x => x.ItemCat,
                        principalTable: "tblInvDefCategory",
                        principalColumn: "ItemCatCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemMaster_tblInvDefClass_ItemClass",
                        column: x => x.ItemClass,
                        principalTable: "tblInvDefClass",
                        principalColumn: "ItemClassCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemMaster_tblInvDefSubCategory_ItemSubCat",
                        column: x => x.ItemSubCat,
                        principalTable: "tblInvDefSubCategory",
                        principalColumn: "ItemSubCatCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemMaster_tblInvDefSubClass_ItemSubClass",
                        column: x => x.ItemSubClass,
                        principalTable: "tblInvDefSubClass",
                        principalColumn: "ItemSubClassCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemMaster_tblInvDefUOM_ItemBaseUnit",
                        column: x => x.ItemBaseUnit,
                        principalTable: "tblInvDefUOM",
                        principalColumn: "UOMCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblInvDefWarehouseTest",
                columns: table => new
                {
                    WHCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WHDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WHAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WHCity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WHState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WHIncharge = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WHBranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvDistGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WhAllowDirectPur = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvDefWarehouseTest", x => x.WHCode);
                });

            migrationBuilder.CreateTable(
                name: "tblPopTrnPurchaseOrderHeader",
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPopTrnPurchaseOrderHeader", x => x.TranNumber);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysCompany_CompCode",
                        column: x => x.CompCode,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysCurrencyCode_TranCurrencyCode",
                        column: x => x.TranCurrencyCode,
                        principalTable: "tblErpSysCurrencyCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysLogin_ClosedBy",
                        column: x => x.ClosedBy,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysLogin_TranCreateUser",
                        column: x => x.TranCreateUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysLogin_TranLastEditUser",
                        column: x => x.TranLastEditUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysLogin_TranpostUser",
                        column: x => x.TranpostUser,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblErpSysLogin_TranvoidDate",
                        column: x => x.TranvoidDate,
                        principalTable: "tblErpSysLogin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblPopDefVendorPOTermsCode_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "tblPopDefVendorPOTermsCode",
                        principalColumn: "POTermsCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblPopDefVendorShipment_TranShipMode",
                        column: x => x.TranShipMode,
                        principalTable: "tblPopDefVendorShipment",
                        principalColumn: "ShipmentCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderHeader_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpInvItemInventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WHCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QtyOH = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    QtyOnSalesOrder = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    QtyOnPO = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    QtyReserved = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemAvgCost = table.Column<decimal>(type: "numeric(11,5)", nullable: false),
                    ItemLastPOCost = table.Column<decimal>(type: "numeric(11,5)", nullable: false),
                    ItemLandedCost = table.Column<decimal>(type: "numeric(11,5)", nullable: false),
                    MinQty = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    MaxQty = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    EOQ = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpInvItemInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemInventory_tblErpInvItemMaster_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemInventory_tblInvDefWarehouse_WHCode",
                        column: x => x.WHCode,
                        principalTable: "tblInvDefWarehouse",
                        principalColumn: "WHCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpInvItemInventoryHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WHCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TranType = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    TranNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    TranUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TranQty = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    unitConvFactor = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    TranTotQty = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    TranPrice = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemAvgCost = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    TranRemarks = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpInvItemInventoryHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemInventoryHistory_tblErpInvItemMaster_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemInventoryHistory_tblInvDefWarehouse_WHCode",
                        column: x => x.WHCode,
                        principalTable: "tblInvDefWarehouse",
                        principalColumn: "WHCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpInvItemNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoteDates = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpInvItemNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemNotes_tblErpInvItemMaster_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpInvItemsBarcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemUOMFlag = table.Column<short>(type: "smallint", nullable: false),
                    ItemBarcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ItemUOM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpInvItemsBarcode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemsBarcode_tblErpInvItemMaster_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemsBarcode_tblInvDefUOM_ItemUOM",
                        column: x => x.ItemUOM,
                        principalTable: "tblInvDefUOM",
                        principalColumn: "UOMCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblErpInvItemsUOM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemUOMFlag = table.Column<short>(type: "smallint", nullable: false),
                    ItemUOM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ItemConvFactor = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemUOMPrice1 = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemUOMPrice2 = table.Column<decimal>(type: "decimal(12,5)", nullable: false),
                    ItemUOMPrice3 = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    ItemUOMDiscPer = table.Column<decimal>(type: "numeric(6,3)", nullable: false),
                    ItemUOMPrice4 = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    ItemAvgCost = table.Column<decimal>(type: "numeric(11,5)", nullable: false),
                    ItemLastPOCost = table.Column<decimal>(type: "numeric(11,5)", nullable: false),
                    ItemLandedCost = table.Column<decimal>(type: "numeric(11,5)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblErpInvItemsUOM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemsUOM_tblErpInvItemMaster_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblErpInvItemsUOM_tblInvDefUOM_ItemUOM",
                        column: x => x.ItemUOM,
                        principalTable: "tblInvDefUOM",
                        principalColumn: "UOMCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblPopTrnPurchaseOrderDetails",
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
                    table.PrimaryKey("PK_tblPopTrnPurchaseOrderDetails", x => x.TranNumber);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblErpInvItemMaster_TranItemCode",
                        column: x => x.TranItemCode,
                        principalTable: "tblErpInvItemMaster",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblErpSysCompany_CompCode",
                        column: x => x.CompCode,
                        principalTable: "tblErpSysCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblErpSysCompanyBranches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "tblErpSysCompanyBranches",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblInvDefUOM_TranItemUnitCode",
                        column: x => x.TranItemUnitCode,
                        principalTable: "tblInvDefUOM",
                        principalColumn: "UOMCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblPopTrnPurchaseOrderHeader_TranId",
                        column: x => x.TranId,
                        principalTable: "tblPopTrnPurchaseOrderHeader",
                        principalColumn: "TranNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblSndDefVendorMaster_TranVendorCode",
                        column: x => x.TranVendorCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblPopTrnPurchaseOrderDetails_tblSndDefVendorMaster_VendCode",
                        column: x => x.VendCode,
                        principalTable: "tblSndDefVendorMaster",
                        principalColumn: "VendCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemInventory_ItemCode",
                table: "tblErpInvItemInventory",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemInventory_WHCode",
                table: "tblErpInvItemInventory",
                column: "WHCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemInventoryHistory_ItemCode",
                table: "tblErpInvItemInventoryHistory",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemInventoryHistory_WHCode",
                table: "tblErpInvItemInventoryHistory",
                column: "WHCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemMaster_ItemBaseUnit",
                table: "tblErpInvItemMaster",
                column: "ItemBaseUnit");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemMaster_ItemCat",
                table: "tblErpInvItemMaster",
                column: "ItemCat");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemMaster_ItemClass",
                table: "tblErpInvItemMaster",
                column: "ItemClass");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemMaster_ItemSubCat",
                table: "tblErpInvItemMaster",
                column: "ItemSubCat");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemMaster_ItemSubClass",
                table: "tblErpInvItemMaster",
                column: "ItemSubClass");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemMaster_ItemTaxCode",
                table: "tblErpInvItemMaster",
                column: "ItemTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemNotes_ItemCode",
                table: "tblErpInvItemNotes",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemsBarcode_ItemCode",
                table: "tblErpInvItemsBarcode",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemsBarcode_ItemUOM",
                table: "tblErpInvItemsBarcode",
                column: "ItemUOM");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemsUOM_ItemCode",
                table: "tblErpInvItemsUOM",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblErpInvItemsUOM_ItemUOM",
                table: "tblErpInvItemsUOM",
                column: "ItemUOM");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_BranchCode",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_CompCode",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_TranId",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "TranId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_TranItemCode",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "TranItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_TranItemUnitCode",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "TranItemUnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_TranVendorCode",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "TranVendorCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderDetails_VendCode",
                table: "tblPopTrnPurchaseOrderDetails",
                column: "VendCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_BranchCode",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_ClosedBy",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "ClosedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_CompCode",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "CompCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_PaymentID",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_TranCreateUser",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "TranCreateUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_TranCurrencyCode",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "TranCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_TranLastEditUser",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "TranLastEditUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_TranpostUser",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "TranpostUser");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_TranShipMode",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "TranShipMode");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_TranvoidDate",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "TranvoidDate");

            migrationBuilder.CreateIndex(
                name: "IX_tblPopTrnPurchaseOrderHeader_VendCode",
                table: "tblPopTrnPurchaseOrderHeader",
                column: "VendCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblErpInvItemInventory");

            migrationBuilder.DropTable(
                name: "tblErpInvItemInventoryHistory");

            migrationBuilder.DropTable(
                name: "tblErpInvItemNotes");

            migrationBuilder.DropTable(
                name: "tblErpInvItemsBarcode");

            migrationBuilder.DropTable(
                name: "tblErpInvItemsUOM");

            migrationBuilder.DropTable(
                name: "tblInvDefWarehouseTest");

            migrationBuilder.DropTable(
                name: "tblPopTrnPurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "tblErpInvItemMaster");

            migrationBuilder.DropTable(
                name: "tblPopTrnPurchaseOrderHeader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblInvDefSubCategory",
                table: "tblInvDefSubCategory");

            migrationBuilder.AlterColumn<string>(
                name: "SubCatKey",
                table: "tblInvDefSubCategory",
                type: "nvarchar(41)",
                maxLength: 41,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(41)",
                oldMaxLength: 41,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblInvDefSubCategory",
                table: "tblInvDefSubCategory",
                column: "SubCatKey");
        }
    }
}
