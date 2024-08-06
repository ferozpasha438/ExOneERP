using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SND
{
    [Table("tblSndDefAccountlPaycodes")]
    //[Index(nameof(FinPayCode), Name = "IX_tblFinDefAccountlPaycodes_FinPayCode", IsUnique = true)]
    public class TblSndDefAccountlPaycodes : AutoGenerateIdKey<int>
    {
        [StringLength(20)]
        [Key]
        public string SndPayCode { get; set; }
        [ForeignKey(nameof(SndBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string SndBranchCode { get; set; }//Reference  BranchCode
       


        [Required]
        [StringLength(50)]
        public string SndPayName { get; set; }

        [ForeignKey(nameof(SndPayAcIntgrAC))]
        public TblFinDefMainAccounts SndIntMainAccounts { get; set; }
        [StringLength(50)]
        public string SndPayAcIntgrAC { get; set; }//Reference FinAcCode

      



        public bool IsActive { get; set; }
        public bool UseByOtherBranches { get; set; }
        public bool SystemGenCheckBook { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
    }
}
