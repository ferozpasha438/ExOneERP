using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefAccountSubCategory")]
    //[Index(nameof(FinSubCatCode), Name = "IX_tblFinDefAccountSubCategory_FinSubCatCode", IsUnique = true)]
    public class TblFinDefAccountSubCategory : AutoGenerateIdKey<int>
    {
        [ForeignKey(nameof(FinCatCode))]
        public TblFinDefAccountCategory FinAccountCategory { get; set; }
        [StringLength(20)]
        public string FinCatCode { get; set; } //Reference FinCatCode

        [StringLength(20)]
        [Key]
        public string FinSubCatCode { get; set; }
        [Required]
        [StringLength(50)]
        public string FinSubCatName { get; set; }
        [Required]
        public short FinSubCatLastSeq { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }

    }
}
