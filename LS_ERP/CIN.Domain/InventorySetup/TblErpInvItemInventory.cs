

namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.SystemSetup;
    using Microsoft.EntityFrameworkCore;

    [Table("tblErpInvItemInventory")]
    public class TblErpInvItemInventory : AutoActiveGenerateIdAuditableKey<int>
    {

        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        public string ItemCode { get; set; }


        [ForeignKey(nameof(WHCode))]
        public TblInvDefWarehouse InvWarehouses { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal QtyOH { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal QtyOnSalesOrder { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal QtyOnPO { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal QtyReserved { get; set; }

        [Column(TypeName = "numeric(11,5)")]
        [Required]
        public decimal ItemAvgCost { get; set; }


        [Column(TypeName = "numeric(11,5)")]
        [Required]
        public decimal ItemLastPOCost { get; set; }

        [Column(TypeName = "numeric(11,5)")]
        [Required]
        public decimal ItemLandedCost { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal MinQty { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal MaxQty { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal EOQ { get; set; }

    }


    #region Inventory Expiry Mgt and Serial Mgt


    [Table("tblErpInvItemExpiryBatch")]
    [Index(nameof(BatchNumber), Name = "IX_tblErpInvItemExpiryBatch_BatchNumber", IsUnique = true)]
    public class TblErpInvItemExpiryBatch : PrimaryKey<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string PoNumber { get; set; }
        [StringLength(250)]
        public string ItemName { get; set; }
        [StringLength(30)]
        public string BatchNumber { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime MfgDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ExpDate { get; set; }
        public decimal Qty { get; set; }
        public decimal QtyCommitted { get; set; }
        public decimal Available { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
    }


    [Table("tblErpInvGrnItemExpiryBatch")]
    public class TblErpInvGrnItemExpiryBatch : PrimaryKey<int>
    {
        [StringLength(20)]
        public string GrnId { get; set; }

        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string PoNumber { get; set; }
        [StringLength(250)]
        public string ItemName { get; set; }
        [StringLength(30)]
        public string BatchNumber { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime MfgDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ExpDate { get; set; }
        public decimal Qty { get; set; }
        public decimal QtyCommitted { get; set; }
        public decimal Available { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
    }



    [Table("tblErpInvPRItemExpiryBatch")]
    public class TblErpInvPRItemExpiryBatch : PrimaryKey<int>
    {
        [StringLength(20)]
        public string GrnId { get; set; }

        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string PoNumber { get; set; }
        [StringLength(250)]
        public string ItemName { get; set; }
        [StringLength(30)]
        public string BatchNumber { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime MfgDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ExpDate { get; set; }
        public decimal Qty { get; set; }
        public decimal QtyCommitted { get; set; }
        public decimal Available { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
    }




    [Table("tblErpInvItemSerialBatch")]
    [Index(nameof(SerialNumber), Name = "IX_tblErpInvItemSerialBatch_SerialNumber", IsUnique = true)]
    public class TblErpInvItemSerialBatch : PrimaryKey<int>
    {
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(250)]
        public string ItemName { get; set; }
        [StringLength(30)]
        public string SerialNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime PoDate { get; set; }
        [StringLength(50)]
        public string PoNumber { get; set; }
        public bool Committed { get; set; }
        [StringLength(50)]
        public string SalesOrdNum { get; set; }
    }

    [Table("tblErpInvItemSpecification")]
    public class TblErpInvItemSpecification : PrimaryKey<int>
    {
        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
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
}
