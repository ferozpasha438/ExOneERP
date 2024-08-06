using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnBankReconciliation")]
    [Index(nameof(VoucherNumber), Name = "IX_tblFinTrnBankReconciliation_VoucherNumber", IsUnique = false)]
    public class TblFinTrnBankReconciliation : PrimaryKey<int>
    {
        [StringLength(50)]
        public string SpVoucherNumber { get; set; }

        [StringLength(50)]
        public string VoucherNumber { get; set; }
        [StringLength(20)]
        public string PayCode { get; set; }
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }

        public DateTime Date { get; set; }    

        [StringLength(50)]
        public string DocNum { get; set; }
       
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Narration { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool Posted { get; set; }
        public bool Void { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? CDate { get; set; }        
    }
}
