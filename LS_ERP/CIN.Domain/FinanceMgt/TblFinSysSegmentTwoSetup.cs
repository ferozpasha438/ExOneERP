using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinSysSegmentTwoSetup")]
    public class TblFinSysSegmentTwoSetup : PrimaryKey<int>
    {
        [Required]
        [StringLength(50)]
        public string Seg2Code { get; set; }
        [Required]
        [StringLength(150)]
        public string Seg2Name { get; set; }
        [StringLength(150)]
        public string Seg2Name2 { get; set; }
        public bool IsActive { get; set; }
    }
}
