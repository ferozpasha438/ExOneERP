using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysLogin")]
    [Index(nameof(UserName), Name = "IX_tblErpSysLogin_UserName", IsUnique = true)]
    //[Index(nameof(LoginId), Name = "IX_tblErpSysLogin_LoginId", IsUnique = true)]
    public class TblErpSysLogin : PrimaryKey<int>
    {
        [StringLength(128)]
        [Required]
        public string LoginId { get; set; }
        [StringLength(128)]
        [Required]
        public string Password { get; set; }
        [StringLength(100)]
        [Required]
        public string UserName { get; set; }
        [StringLength(15)]
        public string UserType { get; set; } //Admin, Manager, etc
        [StringLength(256)]
        public string UserEmail { get; set; }
        [StringLength(256)]
        public string SwpireCardId { get; set; }

        [ForeignKey(nameof(PrimaryBranch))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string PrimaryBranch { get; set; } // Ref branchCode
        [StringLength(128)]
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsLoginAllow { get; set; }
        public bool IsSigned { get; set; }

        [StringLength(50)]
        public string SiteCode { get; set; }
        public string LoginType { get; set; } //Employee,Teacher,Driver

    }
}
