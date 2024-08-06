using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN.Domain.SystemSetup;

namespace CIN.Domain.PurchaseMgt
{

    [Table("tblPurTrnApprovals")]

    public class TblPurTrnApprovals : PrimaryKey<int>
    {
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        [StringLength(5)]
        public string ServiceType { get; set; }         //ENQ,EST,
        [StringLength(20)]
        public string ServiceCode { get; set; }

        public int AppAuth { get; set; }
        [StringLength(500)]
        public string AppRemarks { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
