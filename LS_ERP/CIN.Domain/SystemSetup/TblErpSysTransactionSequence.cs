using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysTransactionSequence")]
    public class TblErpSysTransactionSequence : PrimaryKey<int>
    {
        [ForeignKey(nameof(TransactionCode))]
        public TblErpSysTransactionCode SysTransactionCode { get; set; }
        [StringLength(100)]
        public string TransactionCode { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
        public int LastSeqNum { get; set; }
        [StringLength(10)]
        public string PrefixCode { get; set; }
        public bool PrefixFinYear { get; set; }
        public bool ResetAfterFYClosing { get; set; }
    }
}
