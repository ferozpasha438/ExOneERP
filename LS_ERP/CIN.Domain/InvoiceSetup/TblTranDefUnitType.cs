using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblTranDefUnitType")]
    public class TblTranDefUnitType : PrimaryKey<int>
    {
        [Required]
        [StringLength(150)]
        public string NameEN { get; set; }
        [Required]
        [StringLength(150)]
        public string NameAR { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsDefaultConfig { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
