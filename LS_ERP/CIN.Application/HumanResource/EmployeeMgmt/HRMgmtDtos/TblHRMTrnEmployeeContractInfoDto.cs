using AutoMapper;
using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeContractInfo))]
    public class TblHRMTrnEmployeeContractInfoDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Branch
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        //Grade
        [Required]
        [StringLength(20)]
        public string GradeCode { get; set; }

        //Position
        [Required]
        [StringLength(20)]
        public string PositionCode { get; set; }

        //Payroll Group
        [Required]
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //HolidayCalendar
        [Required]
        [StringLength(20)]
        public string HolidayCalendarCode { get; set; }
    }
}
