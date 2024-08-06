using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN.Domain.SystemSetup;
using CIN.Domain.OpeartionsMgt;

namespace CIN.Domain.InvoiceSetup
{
    [Table("TblFinTrnAdvanceWallet")]
    public class TblFinTrnAdvanceWallet : AuditableCreatedEntity<int>
    {
        public int PaymentId { get; set; }
        public int SNDId { get; set; }
        public int SNDInvNum { get; set; }
        public DateTime? TranDate { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        //[ForeignKey(nameof(CustCode))]
        //public TblSndDefCustomerMaster SndCustomerMaster { get; set; }
        [StringLength(20)]
        public string CustCode { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal AdvAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18, 3)")]
        public decimal AppliedAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18, 3)")]
        public decimal PostedAmount { get; set; } = 0;

        [StringLength(20)]
        public string PayCode { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }
        [StringLength(250)]
        public string Notes { get; set; }
        [StringLength(50)]
        public string PreparedBy { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; }
        [StringLength(50)]
        public string DocNum { get; set; }
         [StringLength(50)]
        public string InvoiceNumber { get; set; }

    }
}
