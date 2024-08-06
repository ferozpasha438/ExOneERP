using AutoMapper;
using CIN.Application.Common.Models;
using CIN.Domain.TimeAndAttendance.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos
{
    [AutoMap(typeof(TblTNATrnEmployeeRoaster))]
    public class TblTNATrnEmployeeRoasterDto : AuditableEntityDto<int>
    {
        public TblTNATrnEmployeeRoasterDto()
        {
            RoasterRows = new List<RoasterRow>();
        }
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Employee Name
        [StringLength(100)]
        public string EmployeeName { get; set; }

        //Branch Code
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        //Branch Name
        [StringLength(100)]
        public string BranchName { get; set; }

        //Payroll Group
        [Required]
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //PayrollGroup Name
        [StringLength(100)]
        public string PayrollGroupName { get; set; }

        //List Roaster Rows 
        public List<RoasterRow> RoasterRows { get; set; }

        public bool IsShiftApplicable { get; set; }
    }

    public class BaseEmployeeRoasterDto
    {
        public BaseEmployeeRoasterDto()
        {
            EmployeeRoasters = new List<TblTNATrnEmployeeRoasterDto>();
            RoasterColumns = new List<RoasterColumn>();
        }
        //List of Employees with Roaster Details
        public List<TblTNATrnEmployeeRoasterDto> EmployeeRoasters { get; set; }

        //List Roaster Columns 
        public List<RoasterColumn> RoasterColumns { get; set; }
    }
}
