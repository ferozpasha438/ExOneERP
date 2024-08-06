using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinSysBatchSetup")]
    public class TblFinSysBatchSetup : PrimaryKey<int>
    {
        [Required]
        [StringLength(50)]
        public string BatchCode { get; set; }
        [Required]
        [StringLength(150)]
        public string BatchName { get; set; }
        [StringLength(150)]
        public string BatchName2 { get; set; }
        public bool IsActive { get; set; }
    }
}
