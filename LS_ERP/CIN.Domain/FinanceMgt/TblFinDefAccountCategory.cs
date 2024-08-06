using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefAccountCategory")]
    [Index(nameof(FinCatCode), Name = "IX_tblFinDefAccountCategory_FinCatCode", IsUnique = true)]
    public class TblFinDefAccountCategory : AutoGenerateIdKey<int>
    {
        [StringLength(20)]
        [Key]
        public string FinCatCode { get; set; }
        [Required]
        [StringLength(50)]
        public string FinCatName { get; set; }


        [ForeignKey(nameof(FinType))]
        public TblFinSysAccountType FinAccountType { get; set; }
        [StringLength(15)]
        public string FinType { get; set; } //Reference  TypeCode
        [Required]
        public short FinCatLastSeq { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
    }
}
