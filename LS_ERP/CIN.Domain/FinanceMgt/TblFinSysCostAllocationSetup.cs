using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinSysCostAllocationSetup")]
    public class TblFinSysCostAllocationSetup : PrimaryKey<int>
    {
        [StringLength(100)]
        public string CostType { get; set; }
        [Required]
        [StringLength(50)]
        public string CostCode { get; set; }
        [Required]
        [StringLength(150)]
        public string CostName { get; set; }
        [StringLength(150)]
        public string CostName2 { get; set; }
        public bool IsActive { get; set; }

    }
}
