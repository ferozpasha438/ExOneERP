using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefAccountlPaycodes")]
    //[Index(nameof(FinPayCode), Name = "IX_tblFinDefAccountlPaycodes_FinPayCode", IsUnique = true)]
    public class TblFinDefAccountlPaycodes : AutoGenerateIdKey<int>
    {
        [StringLength(20)]
        [Key]
        public string FinPayCode { get; set; }
        [ForeignKey(nameof(FinBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string FinBranchCode { get; set; }//Reference  BranchCode
        [Required]
        [StringLength(10)]
        public string FinPayType { get; set; }
        [Required]
        [StringLength(50)]
        public string FinPayName { get; set; }

        [ForeignKey(nameof(FinPayAcIntgrAC))]
        public TblFinDefMainAccounts FinIntMainAccounts { get; set; }
        [StringLength(50)]
        public string FinPayAcIntgrAC { get; set; }//Reference FinAcCode

        [ForeignKey(nameof(FinPayPDCClearAC))]
        public TblFinDefMainAccounts FinPdcMainAccounts { get; set; }
        [StringLength(50)]
        public string FinPayPDCClearAC { get; set; }//Reference FinAcCode

        public bool IsActive { get; set; }
        public bool UseByOtherBranches { get; set; }
        public bool SystemGenCheckBook { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
    }
}
