using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefBranchesAuthority")]
    public class TblFinDefBranchesAuthority : PrimaryKey<int>
    {
        [ForeignKey(nameof(FinBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode

        //[ForeignKey(nameof(AppAuth))]
        //public TblErpSysLogin SysLogin { get; set; }
        [StringLength(20)]
        public string AppAuth { get; set; } //Reference User Login Table
        public short AppLevel { get; set; }
        public bool AppAuthBV { get; set; }
        public bool AppAuthCV { get; set; }
        public bool AppAuthJV { get; set; }
        public bool AppAuthAP { get; set; }
        public bool AppAuthAR { get; set; }
        public bool AppAuthFA { get; set; }
        public bool AppAuthBR { get; set; }
        public bool AppAuthPurcOrder { get; set; }
        public bool AppAuthPurcRequest { get; set; }
        public bool AppAuthPurcReturn { get; set; }
        public bool AppAuthAdj { get; set; }
        public bool AppAuthIssue { get; set; }
        public bool AppAuthRect { get; set; }
        public bool AppAuthTrans { get; set; }

        public bool IsFinalAuthority { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
    }
}
