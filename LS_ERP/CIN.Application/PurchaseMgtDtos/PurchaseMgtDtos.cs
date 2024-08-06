using AutoMapper;
using CIN.Application.GeneralLedgerDtos;
using CIN.Domain;
using CIN.Domain.PurchaseMgt;
using CIN.Domain.PurchaseSetup;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.PurchaseSetupDtos
{


    [AutoMap(typeof(TblPopTrnPurchaseOrderHeader))]
    public class TblPopTrnPurchaseOrderHeaderDto : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        public string VenCatCode { get; set; }
        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }
        [StringLength(10)]
        public string Trantype { get; set; }

        public DateTime TranDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        //public DateTime CancelDate { get; set; }

        public int CompCode { get; set; }

        [StringLength(20)]
        public string BranchCode { get; set; } //Reference  BranchCode
        [StringLength(50)]
        public string InvRefNumber { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        public string VendName { get; set; }
        public string VendAddress { get; set; }
        public string VATNumber { get; set; }
        //[StringLength(50)]
        //public string RFQContractNum { get; set; }
        [StringLength(50)]
        public string DocNumber { get; set; }

        [StringLength(20)]
        public string PaymentID { get; set; }
        public string POTermsName { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string TAXId { get; set; }
        public sbyte TaxInclusive { get; set; }
        [StringLength(500)]
        public string PONotes { get; set; }
        //[StringLength(100)]
        //public string TranBuyer { get; set; }
        public Boolean IsApproved { get; set; }

        //public DateTime TranCreateUserDate { get; set; }

        public int TranCreateUser { get; set; }

        //public DateTime TranLastEditDate { get; set; }

        public int TranLastEditUser { get; set; }
        //public Boolean TranPostStatus { get; set; }

        //public DateTime TranPostDate { get; set; }

        public int TranpostUser { get; set; }
        //public Boolean TranVoidStatus { get; set; }

        //public DateTime TranVoidUser { get; set; }

        public int TranvoidDate { get; set; }

        [StringLength(20)]
        public string TranShipMode { get; set; }

        public int TranCurrencyCode { get; set; }

        //public decimal ExRate { get; set; }



        //public decimal OHCharges { get; set; }



        //public DateTime POClosedDate { get; set; }

        public int ClosedBy { get; set; }
        //public Boolean ForeClosed { get; set; }
        //public Boolean Closed { get; set; }
        public string PurchaseOrderNO { get; set; }
        public string PurchaseRequestNO { get; set; }


        public decimal TranTotalCost { get; set; }

        public decimal TranDiscPer { get; set; }

        public decimal TranDiscAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OHCharges { get; set; }

        public decimal Taxes { get; set; }
        [StringLength(20)]
        public string WHCode { get; set; } //Reference  BranchCode
        public string WHName { get; set; }
        public string TotalAmountWord { get; set; }
        public string ShipmentMode { get; set; }
        public string FromPr { get; set; }
        public string Approvers { get; set; }
        public DateTime? ApproverDate { get; set; }
        public short ExpSerial { get; set; } //1 = exp, 2 = serial, 3 = both

    }
    [AutoMap(typeof(TblPopTrnGRNHeader))]
    public class TblPopTrnGRNHeaderDto : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        public string VenCatCode { get; set; }
        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }
        [StringLength(10)]
        public string Trantype { get; set; }

        public DateTime TranDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        //public DateTime CancelDate { get; set; }

        public int CompCode { get; set; }

        [StringLength(20)]
        public string BranchCode { get; set; } //Reference  BranchCode
        [StringLength(50)]
        public string InvRefNumber { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        //[StringLength(50)]
        //public string RFQContractNum { get; set; }
        [StringLength(50)]
        public string DocNumber { get; set; }

        [StringLength(20)]
        public string PaymentID { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string TAXId { get; set; }
        public sbyte TaxInclusive { get; set; }
        [StringLength(500)]
        public string PONotes { get; set; }
        //[StringLength(100)]
        //public string TranBuyer { get; set; }
        public Boolean IsApproved { get; set; }

        //public DateTime TranCreateUserDate { get; set; }

        public int TranCreateUser { get; set; }

        //public DateTime TranLastEditDate { get; set; }

        public int TranLastEditUser { get; set; }
        //public Boolean TranPostStatus { get; set; }

        //public DateTime TranPostDate { get; set; }

        public int TranpostUser { get; set; }
        //public Boolean TranVoidStatus { get; set; }

        //public DateTime TranVoidUser { get; set; }

        public int TranvoidDate { get; set; }

        [StringLength(20)]
        public string TranShipMode { get; set; }

        public int TranCurrencyCode { get; set; }

        //public decimal ExRate { get; set; }



        //public decimal OHCharges { get; set; }



        //public DateTime POClosedDate { get; set; }

        public int ClosedBy { get; set; }
        //public Boolean ForeClosed { get; set; }
        //public Boolean Closed { get; set; }
        public string PurchaseOrderNO { get; set; }
        public string PurchaseRequestNO { get; set; }


        public decimal TranTotalCost { get; set; }

        public decimal TranDiscPer { get; set; }

        public decimal TranDiscAmount { get; set; }

        public decimal Taxes { get; set; }
        [StringLength(20)]
        public string WHCode { get; set; } //Reference  BranchCode
    }
    public class ProductUnitPriceDTO
    {

        public string tranItemCode { get; set; }
        public string tranItemName { get; set; }
        public string tranItemUnitCode { get; set; }
        public string tranItemCost { get; set; }
        public decimal tranItemUomFactor { get; set; }
        public decimal ItemAvgcost { get; set; }


    }
    public class ProductVendorDTO
    {
        public string VendName { get; set; }
        public string PoTermsCode { get; set; }

    }
    public class ProductTaxDTO
    {
        public decimal ItemTaxperc { get; set; }
        public string ShortName { get; set; }


    }
    public class ItemTaxDTO
    {
        public string Unit { get; set; }

        public string price { get; set; }
        public string Desc { get; set; }


    }
    public class TblPurchaseReturntDto : TblPopTrnPurchaseOrderHeaderDto
    {

        public List<TblPopTrnPurchaseOrderDetailsDto> itemList { get; set; }
        public List<ShipmentQuantityDto> ShipmentList { get; set; }
        public CommonDataLedgerDto Company { get; set; }
    }

    public class TblGRNDetailsDto : TblPopTrnGRNHeaderDto
    {

        public List<TblPopTrnGRNDetailsDto> itemList { get; set; }
    }




    [AutoMap(typeof(TblPopTrnPurchaseOrderDetails))]
    public class TblPopTrnPurchaseOrderDetailsDto : AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(20)]
        public string TranId { get; set; }

        [StringLength(20)]

        public string TranNumber { get; set; }

        //public DateTime TranDate { get; set; }



        //[StringLength(20)]
        //public string VendCode { get; set; }

        //public int CompCode { get; set; }

        //[StringLength(20)]
        //public string BranchCode { get; set; } //Reference  BranchCode

        //[StringLength(20)]
        //public string TranVendorCode { get; set; }

        public sbyte ItemTracking { get; set; }

        public string TranItemCode { get; set; }
        [StringLength(100)]
        public string TranItemName { get; set; }

        [StringLength(100)]
        public string TranItemName2 { get; set; }

        public decimal TranItemQty { get; set; }
        //public decimal OldTranItemQty { get; set; }
        [StringLength(10)]

        public string TranItemUnitCode { get; set; }

        public decimal TranUOMFactor { get; set; }

        public decimal TranItemCost { get; set; }

        public decimal TranTotCost { get; set; }

        public decimal DiscPer { get; set; }

        public decimal DiscAmt { get; set; }

        public decimal ItemTax { get; set; }

        public decimal ItemTaxPer { get; set; }

        public decimal TaxAmount { get; set; }

        //public decimal POQtyReceived { get; set; }

        //public decimal POQtyReceiving { get; set; }

        //public decimal POQtyCancel { get; set; }

        //public decimal POLineCost1 { get; set; }

        //public decimal POLineCost2 { get; set; }

        //public decimal POOHCostPerItem { get; set; }

        //public decimal POLandedCost { get; set; }

        //public decimal POLandedCostPerItem { get; set; }
        //public Boolean TranVoidStatus { get; set; }
        //public Boolean TranPostStatus { get; set; }
        //public Boolean ForeClosed { get; set; }
        //public Boolean Closed { get; set; }
        public int BalQty { get; set; }
        public int PreBalQty { get; set; }
        public decimal? ReceivedQty { get; set; }
        public decimal? ReturnedQty { get; set; }
        public string ItemDescription { get; set; }


    }


    public class ShipmentQuantityDto
    {
        public decimal TranItemQty { get; set; }
        public decimal TranItemCost { get; set; }
        public decimal TranTotCost { get; set; }
        public decimal BalQty { get; set; }
        public decimal? ReceivedQty { get; set; }
        public decimal? ReceivingQty { get; set; }
    }

    [AutoMap(typeof(TblPopTrnGRNDetails))]
    public class TblPopTrnGRNDetailsDto : AutoActiveGenerateIdKeyDto<int>
    {




        [StringLength(20)]
        public string TranId { get; set; }

        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }


        public sbyte ItemTracking { get; set; }

        public string TranItemCode { get; set; }
        [StringLength(100)]
        public string TranItemName { get; set; }

        [StringLength(100)]
        public string TranItemName2 { get; set; }

        public decimal TranItemQty { get; set; }
        [StringLength(10)]

        public string TranItemUnitCode { get; set; }

        public decimal TranUOMFactor { get; set; }

        public decimal TranItemCost { get; set; }

        public decimal TranTotCost { get; set; }

        public decimal DiscPer { get; set; }

        public decimal DiscAmt { get; set; }

        public decimal ItemTax { get; set; }

        public decimal ItemTaxPer { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ReceivingQty { get; set; }
        public decimal BalQty { get; set; }
        public decimal ReceivedQty { get; set; }





    }

    [AutoMap(typeof(TblPopTrnPurchaseReturnHeader))]
    public class TblPopTrnPurchaseReturnHeaderDto : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        public string VenCatCode { get; set; }
        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }
        [StringLength(10)]
        public string Trantype { get; set; }

        public DateTime? TranDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        //public DateTime CancelDate { get; set; }

        public int CompCode { get; set; }

        [StringLength(20)]
        public string BranchCode { get; set; } //Reference  BranchCode
        [StringLength(50)]
        public string InvRefNumber { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        //[StringLength(50)]
        //public string RFQContractNum { get; set; }
        [StringLength(50)]
        public string DocNumber { get; set; }

        [StringLength(20)]
        public string PaymentID { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string TAXId { get; set; }
        public sbyte TaxInclusive { get; set; }
        [StringLength(500)]
        public string PONotes { get; set; }




        public int TranCreateUser { get; set; }



        public int TranLastEditUser { get; set; }

        public int TranpostUser { get; set; }

        public int TranvoidDate { get; set; }

        [StringLength(20)]
        public string TranShipMode { get; set; }

        public int TranCurrencyCode { get; set; }


        public int ClosedBy { get; set; }

        public string PurchaseOrderNO { get; set; }
        public string PurchaseRequestNO { get; set; }

        public bool IsApproved { get; set; }
        [StringLength(20)]
        public string WHCode { get; set; }
        public decimal TranTotalCost { get; set; }

    }

    public class ProductPRUnitPriceDTO
    {

        public string tranItemCode { get; set; }
        public string tranItemName { get; set; }
        public string tranItemUnitCode { get; set; }
        public string tranItemCost { get; set; }
        public decimal tranItemUomFactor { get; set; }
        public decimal ItemAvgcost { get; set; }


    }

    public class TblPurchaseReturListnDto : TblPopTrnPurchaseReturnHeaderDto
    {

        public List<TblPopTrnPurchaseReturnDetailsDto> itemList { get; set; }
    }
    [AutoMap(typeof(TblPopTrnPurchaseReturnDetails))]
    public class TblPopTrnPurchaseReturnDetailsDto : AutoActiveGenerateIdKeyDto<int>
    {




        [StringLength(20)]
        public string TranId { get; set; }

        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }


        public sbyte ItemTracking { get; set; }

        public string TranItemCode { get; set; }
        [StringLength(100)]
        public string TranItemName { get; set; }

        [StringLength(100)]
        public string TranItemName2 { get; set; }

        public decimal TranItemQty { get; set; }
        [StringLength(10)]

        public string TranItemUnitCode { get; set; }

        public decimal TranUOMFactor { get; set; }

        public decimal TranItemCost { get; set; }

        public decimal TranTotCost { get; set; }

        public decimal DiscPer { get; set; }

        public decimal DiscAmt { get; set; }

        public decimal ItemTax { get; set; }

        public decimal ItemTaxPer { get; set; }

        public decimal TaxAmount { get; set; }



    }
    public class TblPurchaseorderQtyUpdateDto
    {

        public string ItemAvgCost { get; set; }
        public decimal QtyOnPO { get; set; }
        public decimal QtyReserved { get; set; }
        public decimal InvItemAvgCost { get; set; }
        public decimal ItemLastPOCost { get; set; }
    }
    public class PurchaseOrderPaginationDto : TblPopTrnPurchaseOrderHeaderDto
    {
        public bool CanEditSurveyForm { get; set; }
        public TblPurAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool HasAuthority { get; set; }
        public bool IsSettled { get; set; }
        public bool CanSettle { get; set; }
        public bool ISGRN { get; set; }

        public List<string> ItemNames { get; set; }


    }
    public class GrnPaginationDto : TblPopTrnGRNHeaderDto
    {
        public bool CanEditSurveyForm { get; set; }
        public TblPurAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool HasAuthority { get; set; }
        public bool IsSettled { get; set; }
        public bool CanSettle { get; set; }
        public List<string> ItemNames { get; set; }
    }
    public class PurchaseRequestPaginationDto : TblPopTrnPurchaseOrderHeaderDto
    {
        public bool CanEditSurveyForm { get; set; }
        public TblPurAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool HasAuthority { get; set; }
        //public bool CanSettle { get; set; }
        public bool IsSettled { get; set; }
        public bool CanSettle { get; set; }
        public List<string> ItemNames { get; set; }

    }
    public class PurchaseReturnPaginationDto : TblPopTrnPurchaseReturnHeaderDto
    {
        public bool CanEditSurveyForm { get; set; }
        public TblPurAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool HasAuthority { get; set; }
        public bool PurchaseReturn { get; set; }
        public bool IsSettled { get; set; }
        public bool CanSettle { get; set; }
        public List<string> ItemNames { get; set; }


    }
}
