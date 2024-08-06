using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblTranDefProductType")]
    public class TblTranDefProductType : PrimaryKey<int>
    {
        [Required]
        [StringLength(150)]
        public string NameEN { get; set; }
        [Required]
        [StringLength(150)]
        public string NameAR { get; set; }
        public bool? IsDefaultConfig { get; set; }
        public int? CompanyId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
