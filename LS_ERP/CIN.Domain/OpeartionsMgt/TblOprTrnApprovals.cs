using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOprTrnApprovals")]
   
    public class TblOprTrnApprovals : PrimaryKey<int>
    {
        


        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        [StringLength(10)]
        public string ServiceType { get; set; }         //ENQ,EST,
        [StringLength(30)]
        public string ServiceCode { get; set; }

         public int AppAuth { get; set; }
        [StringLength(500)]
        public string AppRemarks { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
