using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysZoneSetting")]
    [Index(nameof(Code), Name = "IX_tblErpSysZoneSetting_Code", IsUnique = true)]
    public class TblErpSysZoneSetting : PrimaryKey<int>
    {
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string NameAR { get; set; }
        public bool IsActive { get; set; }
    }
}
