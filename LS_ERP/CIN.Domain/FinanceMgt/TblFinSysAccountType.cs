using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinSysAccountType")]
    public class TblFinSysAccountType : AutoGenerateIdKey<int>
    {
        [Required]
        [StringLength(15)]
        [Key]
        public string TypeCode { get; set; }
        [StringLength(2)]
        [Column(TypeName = "nchar(2)")]
        public string TypeBal { get; set; }

    }
}
