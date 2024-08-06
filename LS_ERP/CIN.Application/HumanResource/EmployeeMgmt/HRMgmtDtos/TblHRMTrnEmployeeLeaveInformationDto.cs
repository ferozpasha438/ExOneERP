using AutoMapper;
using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeLeaveInformation))]
    public class TblHRMTrnEmployeeLeaveInformationDto : AuditableCreatedEntityDto<int>
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(20)]
        public string TemplateCode { get; set; }
        [StringLength(100)]
        public string TemplateName { get; set; }
        [Required]
        [StringLength(20)]
        public string LeaveTypeCode { get; set; }
        [StringLength(256)]
        public string LeaveTypeName { get; set; }
        public decimal Availed { get; set; } = 0;
        public decimal Assigned { get; set; } = 0;
        public DateTime TranDate { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        //Accrual or Pro-Rata
        public int Type { get; set; }
    }

    public class BaseEmployeeLeaveInformationDto
    {
        [Required]
        [StringLength(20)]
        public string LeaveTemplateCode { get; set; }
        public List<TblHRMTrnEmployeeLeaveInformationDto> EmployeeLeaves { get; set; }
    }

    public class EmployeeLeaveInfoFilterDto
    {
        public string GradeCode { get; set; }
        public string PositionCode { get; set; }
    }
}
