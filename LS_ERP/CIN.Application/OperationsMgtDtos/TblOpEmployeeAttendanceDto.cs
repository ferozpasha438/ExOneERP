using AutoMapper;
using CIN.Application;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{

    [AutoMap(typeof(TblOpEmployeeAttendance))]
    public class TblOpEmployeeAttendanceDto : AuditableEntityDto<long>
    {
        public DateTime AttnDate { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ShiftCode { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

     
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public short? ShiftNumber { get; set; }
        public bool isDefaultEmployee { get; set; } = false;

        public bool isPrimarySite { get; set; } = false;
        public bool isDefShiftOff { get; set; } = false;
        public bool isPosted { get; set; } = false;
        [StringLength(20)]
        public string Attendance { get; set; }
        [StringLength(20)]
        public string AltEmployeeNumber { get; set; }
        [StringLength(20)]
        public string AltShiftCode { get; set; }
        public long? RefIdForAlt { get; set; }

        //for new Dashboard
        public bool? IsLate { get; set; } = false;
        public bool? IsLogoutFromShift { get; set; } = false;
        public bool? IsLoginToShift { get; set; } 
        public bool? IsOnBreak { get; set; } = false;
        public bool? IsGeofenseOut { get; set; } = false;
        public short? GeofenseOutCount { get; set; } = 0;
        public string SkillsetCode { get; set; }

    }



    public class DashboardEmployeeAttendanceDto: TblOpEmployeeAttendanceDto
    {
        public bool IsReported { get; set; } = true;
        public bool IsShiftAssigned { get; set; } = true;
        public bool IsOnLeave { get; set; } = false;
        public string EmployeeName { get; set; }
        public string ProjectName { get; set; } 
        public string SiteName { get; set; }
    }

    public class AutoAttendanceDto {

        public List<TblOpEmployeeAttendanceDto> AutoAttendanceData { get; set; }
    }

    public class EmployeeTimeSheetDto: TblOpEmployeeAttendanceDto
    {
        public TimeSpan NWTime { get; set; }
        public TimeSpan LateHrs { get; set; }
        public TimeSpan OverTime { get; set; }
        public int LateDays { get; set; }
        public bool IsWorkedOnday { get; set; }
        public bool IsAbsent { get; set; }
        public int ShiftsCount { get; set; }
        public int OtCount { get; set; }

        public TblOpEmployeeLeavesDto LeavesData { get; set; }
        public TblOpEmployeeTransResignDto TransORresignData { get; set; }

        public bool IsOnLeave { get; set; }=false;
        public bool IsWithDrawn { get; set; } = false;
        public bool IsTransfered { get; set; } = false;
        public bool IsResigned { get; set; } = false;
        public bool IsTerminated { get; set; } = false;

     }



    public class InputGetAttendanceDto
    {
        public string EmployeeNumber { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string AttnDate { get; set; }
        public string ShiftCode { get; set; }
            }    
     public class InputGetRecentPvRequest
    {
        public string EmployeeNumber { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string Date { get; set; }
            }    
    public class OutputGetRecentPvRequest
    {
        public string Message { get; set; }
        public long ReqId { get; set; }
        public EnumPvRequestType? RequestType  { get; set; }
    }

public enum EnumPvRequestType { 
AddResource=1,
RemoveResource=2,
Transfer=3,
TransferWithReplace=4,
SwapResources=5,
ReplaceResource=6

}
    public class InputGetAllAttendanceDto
    {
        public List<InputGetAttendanceDto> InputList { get; set; }
       
            }
 public class InputGetAllAttendanceDtoTest
    {
        public List<InputGetAttendanceDto> InputList { get; set; }
       
            }


    public class InputPostingAttendanceWithDate {
        public DateTime Todate { get; set; }
        public DateTime? Fromdate { get; set; }

        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
       // public List<TblOpEmployeeAttendanceDto> AttendanceList { get; set; }
    }

    public class InputClearMonthlyAttendanceWithDate
    {
        public DateTime Fromdate { get; set; }
        public DateTime? Todate { get; set; }

        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }
}
