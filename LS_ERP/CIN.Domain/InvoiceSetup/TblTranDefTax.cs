using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblTranDefTax")]
    public class TblTranDefTax : PrimaryKey<int>
    {
        public int? CompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }
        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TaxTariffPercentage { get; set; }
        public bool? IsDefault { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
