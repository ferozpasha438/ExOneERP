using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysUserBranch")]
    public class TblErpSysUserBranch : AuditableActiveEntity<int>
    {
        [ForeignKey(nameof(LoginId))]
        public TblErpSysLogin SysLogin { get; set; }
        public int LoginId { get; set; } //DT
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
    }
}
