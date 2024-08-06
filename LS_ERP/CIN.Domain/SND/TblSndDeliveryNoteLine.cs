using CIN.Domain.InventorySetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain
{
    [Table("tblSndDeliveryNoteLine")]
    public class TblSndDeliveryNoteLine : PrimaryKey<long>
    {
        #region sameAsQuotationItem
        [StringLength(50)]
        public string QuotationNumber { get; set; }

        [ForeignKey(nameof(QuotationId))]
        public TblSndDeliveryNoteHeader Quotation { get; set; }
        public long? QuotationId { get; set; }
        public long? CreditMemoId { get; set; }
        public long? DebitMemoId { get; set; }
        public short? QuotationType { get; set; }
        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster SysItem { get; set; }
        public string UnitType { get; set; }
        [ForeignKey(nameof(UnitType))]
        public TblInvDefUOM SysUnitType { get; set; }
        public string ItemCode { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SubTotal { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? AmountBeforeTax { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TaxAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TotalAmount { get; set; }
        public bool? IsDefaultConfig { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TaxTariffPercentage { get; set; }
        public short? Discount { get; set; }

        

        #endregion


        public long DeliveryNoteId { get; set; }
        [ForeignKey(nameof(DeliveryNoteId))]
        public TblSndDeliveryNoteHeader SysDeliveryNote { get; set; }

        public decimal Delivery { get; set; }
        public decimal Delivered { get; set; }
        public decimal BackOrder { get; set; }

        public string Remarks { get; set; }
        public bool DelvFlg1 { get; set; }
        public bool DelvFlg2 { get; set; }
    }



}