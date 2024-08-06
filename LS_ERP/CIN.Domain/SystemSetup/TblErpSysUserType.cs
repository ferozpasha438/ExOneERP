using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysUserType")]
    [Index(nameof(UerType), Name = "IX_tblErpSysUserType_UerType", IsUnique = true)]
    public class TblErpSysUserType : PrimaryKey<int>
    {
        [StringLength(20)]
        public string UerType { get; set; }
    }
}
