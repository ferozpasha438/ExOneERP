using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefMainAccounts")]
    [Index(nameof(FinAcCode), Name = "IX_tblFinDefMainAccounts_FinAcCode", IsUnique = true)]
    public class TblFinDefMainAccounts : AutoGenerateIdKey<int>
    {
        [StringLength(50)]
        [Key]
        public string FinAcCode { get; set; }
        [Required]
        [StringLength(200)]
        public string FinAcName { get; set; }
        [StringLength(200)]
        public string FinAcDesc { get; set; }
        [StringLength(50)]
        public string FinAcAlias { get; set; }
        public bool FinIsPayCode { get; set; }

        [StringLength(10)]
        public string FinPayCodetype { get; set; }
        public bool FinIsIntegrationAC { get; set; }
        [Required]
        [StringLength(15)]
        public string Fintype { get; set; } //Reference  TypeCode
        [Required]
        [StringLength(20)]
        public string FinCatCode { get; set; } //Reference  tblFinDefAccountCategory FinCatCode
        [Required]
        [StringLength(20)]
        public string FinSubCatCode { get; set; } //Reference  tblFinDefAccountSubCategory FinSubCatCode        

        public bool FinIsRevenue { get; set; }
        [StringLength(15)]
        public string FinIsRevenuetype { get; set; }

        public short FinActLastSeq { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
        public bool? Status { get; set; }

    }
}
