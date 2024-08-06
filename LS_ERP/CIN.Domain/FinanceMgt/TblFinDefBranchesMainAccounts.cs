using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefBranchesMainAccounts")]
    public class TblFinDefBranchesMainAccounts : PrimaryKey<int> //AuditableActiveEntity<int>
    {
        [ForeignKey(nameof(FinAcCode))]
        public TblFinDefMainAccounts FinMainAccounts { get; set; }
        [StringLength(50)]
        public string FinAcCode { get; set; } //Reference FinAcCode

        [ForeignKey(nameof(FinBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }

    }
}
