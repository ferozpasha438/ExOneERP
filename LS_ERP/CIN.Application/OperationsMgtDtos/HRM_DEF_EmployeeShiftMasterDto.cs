using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(HRM_DEF_EmployeeShiftMaster))]
    public class HRM_DEF_EmployeeShiftMasterDto
    {

      
        public long ShiftId { get; set; }
        [StringLength(100)]
        public string ShiftCode { get; set; }
        [StringLength(100)]
        public string ShiftName_EN { get; set; }
        [StringLength(100)]
        public string ShiftName_AR { get; set; }
     
        public TimeSpan? InTime { get; set; }
      
        public TimeSpan? OutTime { get; set; }
      
        public TimeSpan? BreakTime { get; set; }
        public TimeSpan? InGrace { get; set; }
        public TimeSpan? OutGrace { get; set; }
        public TimeSpan? WorkingTime { get; set; }
        public TimeSpan? NetWorkingTime { get; set; }
        public bool? IsOff { get; set; }
        public bool? IsActive { get; set; }
        public bool? CanDelete { get; set; } = false;
    }
    [AutoMap(typeof(HRM_DEF_EmployeeShiftMaster))]
    public class HRM_DEF_EmployeeShiftMasterPaginationDto
    {


        public long ShiftId { get; set; }
        [StringLength(100)]
        public string ShiftCode { get; set; }
        [StringLength(100)]
        public string ShiftName_EN { get; set; }
        [StringLength(100)]
        public string ShiftName_AR { get; set; }

        public string InTime { get; set; }

        public string OutTime { get; set; }

        public string BreakTime { get; set; }
        public string InGrace { get; set; }
        public string OutGrace { get; set; }
        public string WorkingTime { get; set; }
        public string NetWorkingTime { get; set; }
        public bool? IsOff { get; set; }
        public bool? IsActive { get; set; }

    }
    [AutoMap(typeof(HRM_DEF_EmployeeShiftMaster))]
    public class HRM_DEF_EmployeeShiftMasterAddUpdateDto
    {


        public long ShiftId { get; set; }
        [StringLength(100)]
        public string ShiftCode { get; set; }
        [StringLength(100)]
        public string ShiftName_EN { get; set; }
        [StringLength(100)]
        public string ShiftName_AR { get; set; }

        public string InTime { get; set; }

        public string OutTime { get; set; }

        public short BreakTime { get; set; }
        public short InGrace { get; set; }
        public short OutGrace { get; set; }
        public string WorkingTime { get; set; }
        public string NetWorkingTime { get; set; }
        public bool? IsOff { get; set; }
        public bool? IsActive { get; set; }

    }









}
