using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefBankCheckLeaves")]
    public class TblFinDefBankCheckLeaves : PrimaryKey<int>
    {
        [ForeignKey(nameof(FinPayCode))]
        public TblFinDefAccountlPaycodes FinAccountlPaycodes { get; set; }
        [StringLength(20)]
        public string FinPayCode { get; set; } //Reference FinPayCode
        [Required]
        public int StChkNum { get; set; }
        [Required]
        public int EndChkNum { get; set; }
        [StringLength(10)]
        public string CheckLeavePrefix { get; set; }
        public bool IsUsed { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(2)]
        [Column(TypeName = "char(2)")]
        public string TranSource { get; set; }

        [StringLength(25)]
        public string UsedByTranNum { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UsedOn { get; set; }
        public bool IsVoided { get; set; }
        [Column(TypeName = "date")]
        public DateTime? VoidedOn { get; set; }
        [StringLength(20)]
        public string VoidedBy { get; set; }
        public bool IsPDC { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CheckDate { get; set; }
        [StringLength(150)]
        public string IssuedToName { get; set; }
        public bool IsBounced { get; set; }
        [StringLength(50)]
        public string BounceReason { get; set; }
        public bool IsCleared { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ClearedOn { get; set; }
        public bool IsClosed { get; set; }


    }
}
