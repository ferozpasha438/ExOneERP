using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpPvOpenCloseReq")]
    public class TblOpPvOpenCloseReq
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }

        public bool IsSuspendReq { get; set; }
        public bool IsCancelReq { get; set; }
        public bool IsCloseReq { get; set; }
        public bool IsReOpenReq { get; set; }
        public bool IsRevokeSuspReq { get; set; }
        public bool IsExtendProjReq { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExtensionDate { get; set; }


        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int ModifiedBy { get; set; }
        public bool IsApproved { get; set; }


        public int ApprovedBy { get; set; }
        public string FileUrl { get; set; }
        public int? FileUploadBy { get; set; }
    }
}