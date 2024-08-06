using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysTransactionCodes")]
   // [Index(nameof(TransactionCode), Name = "IX_TblErpSysTransactionCode_TransactionCode", IsUnique = false)]
    public class TblErpSysTransactionCode : AutoGenerateIdKey<int>
    {
        [StringLength(100)]
        [Key]
        public string TransactionCode { get; set; }
    }
}
