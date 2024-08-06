using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblSndDefServiceEnquiries")]
    public class TblSndDefServiceEnquiries          // : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnquiryID { get; set; }
        [ForeignKey(nameof(EnquiryNumber))]
        public TblSndDefServiceEnquiryHeader SysEnquiryNumber { get; set; }
        public string EnquiryNumber { get; set; }
        [ForeignKey(nameof(SiteCode))]
        public TblSndDefSiteMaster SysSiteCode { get; set; }
        public string SiteCode { get; set; }
        [ForeignKey(nameof(ServiceCode))]
        public TblSndDefServiceMaster SysServiceCode { get; set; }
        public string ServiceCode { get; set; }

        [ForeignKey(nameof(UnitCode))]
        public TblSndDefUnitMaster SysUnitCode { get; set; }
        public string UnitCode { get; set; }
        public int UnitQuantity { get; set; }
        public int ServiceQuantity { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal PricePerUnit { get; set; }


        [Column(TypeName = "decimal(17,3)")]
        public decimal EstimatedPrice { get; set; }

        public string StatusEnquiry { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public string SurveyorCode { get; set; }





        public bool IsAssignedSurveyor { get; set; }
        public bool IsSurveyInProgress { get; set; }
        public bool IsSurveyCompleted { get; set; }
        public bool IsApproved { get; set; }
    }
}
