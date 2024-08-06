using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.SND
{
    [Table("tblSndTrnApprovals")]
   
    public class TblSndTrnApprovals : PrimaryKey<int>
    {
        


        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        public short ServiceType { get; set; }                        // ref from  EnumSndApprovalServiceType
        

        [StringLength(30)]
        public string ServiceCode { get; set; }

        [ForeignKey(nameof(AppAuth))]
        public TblErpSysLogin SysLogin { get; set; }

        public int AppAuth { get; set; }
        [StringLength(500)]
        public string AppRemarks { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
