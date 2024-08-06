namespace CIN.Domain.PurchaseMgt
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.OpeartionsMgt;
    using CIN.Domain.PurchaseSetup;
    using CIN.Domain.SystemSetup;

    [Table("tblPopTrnGRNHeader")]
    public class TblPopTrnGRNHeader : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        public string VenCatCode { get; set; }
        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }
        [StringLength(10)]
        public string Trantype { get; set; }
        [Column(TypeName = "date")]
        public DateTime TranDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime DeliveryDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime CancelDate { get; set; }
        [ForeignKey(nameof(CompCode))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompCode { get; set; }
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch BranchId { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; } //Reference  BranchCode
        [StringLength(50)]
        public string InvRefNumber { get; set; }
        [ForeignKey(nameof(VendCode))]
        public TblSndDefVendorMaster Vendor { get; set; }
        [StringLength(20)]
        public string VendCode { get; set; }
        [StringLength(50)]
        public string RFQContractNum { get; set; }
        [StringLength(50)]
        public string DocNumber { get; set; }
        [ForeignKey(nameof(PaymentID))]
        public TblPopDefVendorPOTermsCode Payment { get; set; }
        [StringLength(20)]
        public string PaymentID { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string TAXId { get; set; }
        public sbyte TaxInclusive { get; set; }
        [StringLength(500)]
        public string PONotes { get; set; }
        [StringLength(100)]
        public string TranBuyer { get; set; }
        public Boolean IsApproved { get; set; }
        [Column(TypeName = "date")]
        public DateTime TranCreateUserDate { get; set; }
        [ForeignKey(nameof(TranCreateUser))]
        public TblErpSysLogin TranCreateUserID { get; set; }
        public int TranCreateUser { get; set; }
        [Column(TypeName = "date")]
        public DateTime TranLastEditDate { get; set; }
        [ForeignKey(nameof(TranLastEditUser))]
        public TblErpSysLogin TranLastEditUserID { get; set; }
        public int TranLastEditUser { get; set; }
        public Boolean TranPostStatus { get; set; }
        [Column(TypeName = "date")]
        public DateTime TranPostDate { get; set; }
        [ForeignKey(nameof(TranpostUser))]
        public TblErpSysLogin TranpostUserID { get; set; }
        public int TranpostUser { get; set; }
        public Boolean TranVoidStatus { get; set; }
        [Column(TypeName = "date")]
        public DateTime TranVoidUser { get; set; }
        [ForeignKey(nameof(TranvoidDate))]
        public TblErpSysLogin TranvoidDateUser { get; set; }
        public int TranvoidDate { get; set; }
        [ForeignKey(nameof(TranShipMode))]
        public TblPopDefVendorShipment TranShipModeShipment { get; set; }
        [StringLength(20)]
        public string TranShipMode { get; set; }
        [ForeignKey(nameof(TranCurrencyCode))]
        public TblErpSysCurrencyCode TranCurrencyCodeCurrency { get; set; }
        public int TranCurrencyCode { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public decimal ExRate { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal TranTotalCost { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal TranDiscPer { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal TranDiscAmount { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal OHCharges { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal Taxes { get; set; }
        [Column(TypeName = "date")]
        public DateTime POClosedDate { get; set; }
        [ForeignKey(nameof(ClosedBy))]
        public TblErpSysLogin ClosedByUser { get; set; }
        public int ClosedBy { get; set; }
        public Boolean ForeClosed { get; set; }
        public Boolean Closed { get; set; }

        [StringLength(50)]
        public string PurchaseRequestNO { get; set; }

        [StringLength(50)]
        public string PurchaseOrderNO { get; set; }

        [StringLength(20)]
        public string WHCode { get; set; } //Reference  BranchCode
        public bool IsPaid { get; set; }
    }
}
