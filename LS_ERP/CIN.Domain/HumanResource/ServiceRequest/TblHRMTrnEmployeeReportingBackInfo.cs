using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest
{
    [Table("tblHRMTrnEmployeeReportingBackInfo")]
    public class TblHRMTrnEmployeeReportingBackInfo : AuditableEntity<int>
    {
        [ForeignKey(nameof(EmployeeServiceRequestID))]
        public TblHRMTrnEmployeeServiceRequest TrnEmployeeServiceRequest { get; set; }
        [Required]
        public int EmployeeServiceRequestID { get; set; }
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime ReportingDate { get; set; }
        [ForeignKey(nameof(ManagerEmployeeID))]
        public TblHRMTrnPersonalInformation TrnManagerPersonalInformation { get; set; }
        [Required]
        public int ManagerEmployeeID { get; set; }
        [StringLength(500)]
        public string ReportingReason { get; set; }
        public bool IsApprovalLetterRequired { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(80)]
        public string UploadedFileName { get; set; }
        public bool IsJoiningReportSubmitted { get; set; }
        public bool IsAllowedToResumeDuty { get; set; }
        [StringLength(500)]
        public string ActionRequired { get; set; }
    }
}
