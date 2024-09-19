using AutoMapper;
using CIN.Domain.HumanResource.ServiceRequest;
using System;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeReportingBackInfo))]
    public class TblHRMTrnEmployeeReportingBackInfoDto : AuditableEntityDto<int>
    {

        [Required]
        public int EmployeeServiceRequestID { get; set; }

        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime ReportingDate { get; set; }

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
