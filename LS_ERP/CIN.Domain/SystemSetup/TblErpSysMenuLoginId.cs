using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysMenuLoginId")]
    public class TblErpSysMenuLoginId : PrimaryKey<int>
    {
        [ForeignKey(nameof(MenuCode))]
        public TblErpSysMenuOption SysMenuOption { get; set; }
        [StringLength(10)]
        public string MenuCode { get; set; } //ref tblErpSysMenuOption
        [ForeignKey(nameof(LoginId))]
        public TblErpSysLogin SysLogin { get; set; }
        public int LoginId { get; set; } //ref tblErpSysLogin
        public bool IsAllowed { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(20)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

    }
}
