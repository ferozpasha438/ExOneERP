using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblTranDefProduct")]
    public class TblTranDefProduct : PrimaryKey<int>
    {
        [StringLength(150)]
        public string NameEN { get; set; }
        [StringLength(150)]
        public string NameAR { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string ProductCode { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey(nameof(ProductTypeId))]
        public TblTranDefProductType ProductType { get; set; }

        public int? ProductTypeId { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CostPrice { get; set; }

        [ForeignKey(nameof(UnitTypeId))]
        public TblTranDefUnitType  UnitType { get; set; }
        public int? UnitTypeId { get; set; }
        [StringLength(150)]
        public string Barcode { get; set; }
        public bool? IsDefaultConfig { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
