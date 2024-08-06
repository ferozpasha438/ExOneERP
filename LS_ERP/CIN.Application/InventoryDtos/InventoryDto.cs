using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using CIN.Domain.InventorySetup;

namespace CIN.Application.InventoryDtos
{

    [AutoMap(typeof(TblInvDefDistributionGroup))]
    public class TblInvDefDistributionGroupDto : AutoGenerateIdAuditableKeyDto<int>
    {
        [StringLength(20)]
        [Required]
        public string InvDistGroup { get; set; }
        [StringLength(50)]
        [Required]
        public string InvAssetAc { get; set; }
        [Required]
        [StringLength(50)]
        public string InvNonAssetAc { get; set; }

        [StringLength(50)]
        [Required]
        public string InvCashPOAC { get; set; }

        [StringLength(50)]
        [Required]
        public string InvCOGSAc { get; set; }

        [StringLength(50)]

        public string InvAdjAc { get; set; }

        [StringLength(50)]

        public string InvSalesAc { get; set; }
        [StringLength(50)]

        public string InvInTransitAc { get; set; }

        [StringLength(50)]

        public string InvDefaultAPAc { get; set; }
        [StringLength(50)]

        public string InvCostCorAc { get; set; }
        [StringLength(50)]

        public string InvWIPAc { get; set; }
        [StringLength(50)]

        public string InvWriteOffAc { get; set; }

    }


    [AutoMap(typeof(TblInventoryDefDistributionGroup))]
    public class TblInventoryDefDistributionGroupDto : TblInvDefDistributionGroupDto
    {

    }

    [AutoMap(typeof(TblInvDefInventoryConfig))]
    public class TblInvDefInventoryConfigDto : PrimaryKeyDto<int>
    {

        [StringLength(10)]
        [Required]
        public string CentralWHCode { get; set; }
        public bool AutoGenItemCode { get; set; }
        public bool PrefixCatCode { get; set; }

        [StringLength(10)]
        [Required]
        public string NewItemIndicator { get; set; }
        [Required]
        public sbyte ItemLength { get; set; }
        [Required]
        public sbyte CategoryLength { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool CanEdit { get; set; }

    }
    [AutoMap(typeof(TblInvDefUOM))]
    public class TblInvDefUOMDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(10)]
        [Required]
        public string UOMCode { get; set; }
        [StringLength(20)]
        [Required]
        public string UOMName { get; set; }
        [StringLength(20)]
        public string UOMDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
    [AutoMap(typeof(TblInvDefWarehouse))]
    public class TblInvDefWarehouseDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(10)]
        [Required]
        public string WHCode { get; set; }
        [StringLength(50)]
        [Required]
        public string WHName { get; set; }
        [StringLength(50)]
        public string WHDesc { get; set; }
        [StringLength(500)]
        [Required]
        public string WHAddress { get; set; }
        [Required]
        [StringLength(20)]
        public string WHCity { get; set; }
        [StringLength(50)]
        public string WHState { get; set; }
        [Required]
        [StringLength(50)]
        public string WHIncharge { get; set; }
        [StringLength(20)]
        [Required]
        public string WHBranchCode { get; set; }
        [StringLength(20)]
        [Required]
        public string InvDistGroup { get; set; }
        public bool WhAllowDirectPur { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
    [AutoMap(typeof(TblInvDefWarehouseTest))]
    public class TblInvDefWarehouseTestDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(10)]
        public string WHCode { get; set; }
        [StringLength(50)]
        [Required]
        public string WHName { get; set; }
        [StringLength(50)]
        public string WHDesc { get; set; }
        [StringLength(500)]
        [Required]
        public string WHAddress { get; set; }
        [Required]
        [StringLength(20)]
        public string WHCity { get; set; }
        [StringLength(50)]
        public string WHState { get; set; }
        [Required]
        [StringLength(50)]
        public string WHIncharge { get; set; }
        [StringLength(20)]
        public string WHBranchCode { get; set; }
        [StringLength(20)]
        public string InvDistGroup { get; set; }
        public bool WhAllowDirectPur { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    [AutoMap(typeof(TblInvDefCategory))]
    public class tblInvDefCategoryDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(40)]
        [Required]
        public string ItemCatCode { get; set; }
        [StringLength(100)]
        [Required]
        public string ItemCatName { get; set; }
        [StringLength(100)]
        public string ItemCatDesc { get; set; }
        [StringLength(10)]
        public string ItemCatPrefix { get; set; }
        //public int NextItemNumber { get; set; }



    }
    [AutoMap(typeof(TblInvDefSubCategory))]
    public class TblInvDefSubCategoryDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(41)]
        [Key]
        [Required]
        public string SubCatKey { get; set; }
        [StringLength(20)]
        [Required]
        public string ItemSubCatCode { get; set; }
        [StringLength(20)]
        [Required]
        public string ItemCatCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemSubCatName { get; set; }
        [StringLength(50)]
        public string ItemSubCatDesc { get; set; }

    }
    [AutoMap(typeof(TblInvDefClass))]
    public class TblInvDefClassDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        [Key]
        [Required]
        public string ItemClassCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemClassName { get; set; }
        [StringLength(50)]
        public string ItemClassDesce { get; set; }

    }

    [AutoMap(typeof(TblInvDefSubClass))]
    public class TblInvDefSubClassDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        [Key]
        [Required]
        public string ItemSubClassCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemSubClassName { get; set; }
        [StringLength(50)]
        public string ItemSubClassDesce { get; set; }

    }

    [AutoMap(typeof(TblErpInvItemMaster))]
    public class TblErpInvItemMasterDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        [Key]
        public string ItemCode { get; set; }

        [StringLength(20)]
        public string HSNCode { get; set; }

        [StringLength(100)]
        public string ItemDescription { get; set; }

        [StringLength(100)]
        public string ItemDescriptionAr { get; set; }

        [StringLength(250)]
        public string ShortName { get; set; }
        [StringLength(250)]
        public string ShortNameAr { get; set; }
        [StringLength(20)]
        public string ItemCat { get; set; }
        [StringLength(20)]
        public string ItemSubCat { get; set; }
        [StringLength(20)]
        public string ItemClass { get; set; }
        [StringLength(20)]
        public string ItemSubClass { get; set; }
        [StringLength(10)]
        public string ItemBaseUnit { get; set; }
        [StringLength(10)]
        public string ItemAvgCost { get; set; }
        [StringLength(10)]
        public string ItemStandardCost { get; set; }
        [StringLength(20)]
        public string ItemPrimeVendor { get; set; }
        public decimal ItemStandardPrice1 { get; set; }
        public decimal ItemStandardPrice2 { get; set; }
        public decimal ItemStandardPrice3 { get; set; }
        [StringLength(20)]
        public string ItemType { get; set; }
        [StringLength(20)]
        public string ItemTracking { get; set; }
        [StringLength(20)]
        public string ItemWeight { get; set; }
        //[StringLength(20)]
        //public string ItemInputTaxCode { get; set; }
        [StringLength(20)]
        public string ItemTaxCode { get; set; }
        public bool AllowPriceOverride { get; set; }
        public bool AllowDiscounts { get; set; }
        public bool AllowQuantityOverride { get; set; }

    }

    #region InventroyItemTab
    public class TblInventoryItemsDto : TblErpInvItemInventoryDto
    {
        public List<TblErpInvItemInventoryDto> InventoryList { get; set; }
    }


    [AutoMap(typeof(TblErpInvItemInventory))]
    public class TblErpInvItemInventoryDto : AutoActiveGenerateIdKeyDto<int>
    {


        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }
        public decimal QtyOH { get; set; }
        public decimal QtyOnSalesOrder { get; set; }
        public decimal QtyOnPO { get; set; }
        public decimal QtyReserved { get; set; }
        //public decimal ItemAvgCost { get; set; }

        public decimal InvItemAvgCost { get; set; }

        public decimal ItemLastPOCost { get; set; }
        public decimal ItemLandedCost { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal EOQ { get; set; }
    }



    #region Inventory Expiry Mgt and Serial Mgt


    [AutoMap(typeof(TblErpInvItemExpiryBatch))]
    public class TblErpInvItemExpiryBatchDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string PoNumber { get; set; }
        [StringLength(30)]
        public string BatchNumber { get; set; }
        public DateTime MfgDate { get; set; }
        public DateTime ExpDate { get; set; }
        public decimal Qty { get; set; }
        public decimal QtyCommitted { get; set; }
        public decimal Available { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
    }

    public class TblErpInvItemExpiryBatchListDto
    {
        public List<TblErpInvItemExpiryBatchDto> Items { get; set; }
    }


    [AutoMap(typeof(TblErpInvItemSerialBatch))]
    public class TblErpInvItemSerialBatchDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(30)]
        public string SerialNumber { get; set; }
        public DateTime PoDate { get; set; }
        [StringLength(50)]
        public string PoNumber { get; set; }
        public bool Committed { get; set; }
        [StringLength(50)]
        public string SalesOrdNum { get; set; }
    }
    public class TblErpInvItemSerialBatchListDto
    {
        public List<TblErpInvItemSerialBatchDto> SerialList { get; set; }
    }

    public class InvItemSelectListByPoNumberDto
    {
        public List<CustomSelectListItem> Exps { get; set; }
        public List<CustomSelectListItem> Serials { get; set; }
    }




    [AutoMap(typeof(TblErpInvItemSpecification))]
    public class TblErpInvItemSpecificationDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(128)]
        public string Specification { get; set; }
        [StringLength(56)]
        public string Value { get; set; }
        [StringLength(20)]
        public string Unit { get; set; }
        public bool HasWarranty { get; set; }
        [StringLength(20)]
        public string WarrTime { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }

    }



    #endregion


    #endregion 
    #region UOMTab
    public class TblINVTblErpInvItemsUOMDto : TblErpInvItemsUOMDto
    {
        public List<TblErpInvItemsUOMDto> AuthList { get; set; }
    }

    [AutoMap(typeof(TblErpInvItemsUOM))]
    public class TblErpInvItemsUOMDto : AutoActiveGenerateIdKeyDto<int>
    {

        [StringLength(20)]
        public string ItemCode { get; set; }
        public sbyte ItemUOMFlag { get; set; }
        [StringLength(10)]
        public string ItemUOM { get; set; }
        public decimal ItemConvFactor { get; set; }
        public decimal ItemUOMPrice1 { get; set; }
        public decimal ItemUOMPrice2 { get; set; }
        public decimal ItemUOMPrice3 { get; set; }
        public decimal ItemUOMDiscPer { get; set; }
        public decimal ItemUOMPrice4 { get; set; }
        //public decimal ItemAvgCost { get; set; }
        //public decimal ItemLastPOCost { get; set; }
        //public decimal ItemLandedCost { get; set; }
        public decimal ItemUomAvgCost { get; set; }
        public decimal ItemUomLastPOCost { get; set; }
        public decimal ItemUomLandedCost { get; set; }

    }
    #endregion
    #region BarcodeItem
    public class TblBarcodeItemsDto : TblErpInvItemsBarcodeDto
    {
        public List<TblErpInvItemsBarcodeDto> BarCodeList { get; set; }
    }


    [AutoMap(typeof(TblErpInvItemsBarcode))]
    public class TblErpInvItemsBarcodeDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        public sbyte ItemUOMFlag { get; set; }
        [StringLength(25)]
        public string ItemBarcode { get; set; }
        [StringLength(10)]
        public string ItemBarUOM { get; set; }

        //public string ItemUOM { get; set; }
    }
    #endregion 
    #region NotesItem
    public class TblNotesItemsDto : TblErpInvItemNotesDto
    {
        public List<TblErpInvItemNotesDto> NotesList { get; set; }
    }


    [AutoMap(typeof(TblErpInvItemNotes))]
    public class TblErpInvItemNotesDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Notes { get; set; }
        public DateTime NoteDates { get; set; }
    }
    #endregion
    #region ItemHistory
    public class TblHistoryItemsDto : TblErpInvItemInventoryHistoryDto
    {
        public List<TblErpInvItemInventoryHistoryDto> HistoryList { get; set; }
    }


    [AutoMap(typeof(TblErpInvItemInventoryHistory))]
    public class TblErpInvItemInventoryHistoryDto : AutoActiveGenerateIdKeyDto<int>
    {

        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }
        public DateTime TranDate { get; set; }
        [StringLength(3)]
        public string TranType { get; set; }
        [StringLength(20)]
        public string TranNumber { get; set; }
        [StringLength(10)]
        public string TranUnit { get; set; }
        public decimal TranQty { get; set; }
        public decimal unitConvFactor { get; set; }
        public decimal TranTotQty { get; set; }
        public decimal TranPrice { get; set; }
        public decimal ItemAvgCost { get; set; }
        [StringLength(20)]
        public string TranRemarks { get; set; }
    }
    #endregion 
    public class ItemGenerateNumberDto
    {

        public int ItemCode { get; set; }
        //public List<ItemGenerateNumberDto> ItemGenerateNumber { get; set; }
    }
    #region EditInventoryItem
    public class TblEditInventoryItemDto : TblErpInvItemMasterDto
    {
        public List<TblErpInvItemInventoryDto> InventoryList { get; set; }
        public List<TblErpInvItemsUOMDto> AuthList { get; set; }
        public List<TblErpInvItemsBarcodeDto> BarcodeList { get; set; }
        public List<TblErpInvItemNotesDto> NotesList { get; set; }
        public List<TblErpInvItemInventoryHistoryDto> HistoryList { get; set; }

    }
    #endregion
    #region For Binding To SubCategoty Page

    public class AccountAccountCodeDto
    {
        [StringLength(50)]
        public string FinAcCode { get; set; }
        [Required]
        [StringLength(200)]
        public string FinAcName { get; set; }
        [StringLength(200)]
        public string FinAcDesc { get; set; }
        [StringLength(50)]
        public string FinAcAlias { get; set; }
    }
    public class InvSubCategoryDto
    {
        [StringLength(20)]
        public string ItemSubCatCode { get; set; }
        [StringLength(20)]
        public string ItemCatCode { get; set; }
        [StringLength(50)]
        public string ItemSubCatName { get; set; }
        public List<AccountAccountCodeDto> List { get; set; }

    }
    public class InvCategoryDto
    {
        [StringLength(40)]
        public string ItemCatCode { get; set; }
        [StringLength(100)]
        public string ItemCatName { get; set; }

        public List<InvSubCategoryDto> List { get; set; }
    }

    public class InvRootSubCategoryDto
    {
        //public int Sequence { get; set; }
        public string Name { get; set; }
        public List<InvCategoryDto> List { get; set; }

    }

    public class InvRootSubCategoryListDto
    {
        //public bool FInSysGenAcCode { get; set; }
        public List<InvRootSubCategoryDto> List { get; set; }
    }

    #endregion
}
