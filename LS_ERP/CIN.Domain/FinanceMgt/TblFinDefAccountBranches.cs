using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefAccountBranches")]
    public class TblFinDefAccountBranches : PrimaryKey<int>
    {
        [ForeignKey(nameof(FinBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode
        [Required]
        [StringLength(20)]
        public string FinBranchPrefix { get; set; }
        [Required]
        [StringLength(50)]
        public string FinBranchName { get; set; }
        [Required]
        [StringLength(150)]
        public string FinBranchDesc { get; set; }
        [Required]
        [StringLength(500)]
        public string FinBranchAddress { get; set; }
        [Required]
        [StringLength(10)]
        public string FinBranchType { get; set; }
        public bool FinBranchIsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }



        //[StringLength(20)]
        //public string AppAuth01 { get; set; }
        //public bool AppAuth01BV { get; set; }
        //public bool AppAuth01CV { get; set; }
        //public bool AppAuth01JV { get; set; }
        //public bool AppAuth01AP { get; set; }
        //public bool AppAuth01AR { get; set; }
        //public bool AppAuth01FA { get; set; }
        //public bool AppAuth01BR { get; set; }
        //[StringLength(20)]
        //public string AppAuth02 { get; set; }
        //public bool AppAuth02BV { get; set; }
        //public bool AppAuth02CV { get; set; }
        //public bool AppAuth02JV { get; set; }
        //public bool AppAuth02AP { get; set; }
        //public bool AppAuth02AR { get; set; }
        //public bool AppAuth02FA { get; set; }
        //public bool AppAuth02BR { get; set; }
        //[StringLength(20)]
        //public string AppAuth03 { get; set; }
        //public bool AppAuth03BV { get; set; }
        //public bool AppAuth03CV { get; set; }
        //public bool AppAuth03JV { get; set; }
        //public bool AppAuth03AP { get; set; }
        //public bool AppAuth03AR { get; set; }
        //public bool AppAuth03FA { get; set; }
        //public bool AppAuth03BR { get; set; }

    }
}
