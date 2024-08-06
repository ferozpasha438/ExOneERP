using AutoMapper;
using CIN.Application.SystemSetupDtos;
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


    #region AttendanceReportForPayroll
    public class Output_OpAttendanceReportForPayRollPeriodDto
    {
        public bool IsValidReq { get; set; }
        public string ErrorMsg { get; set; }


        public List<Column_OpAttendanceReportForPayRollPeriodDto> Columns { get; set; }   //columns
        public List<Row_OpAttendanceReportForPayRollPeriodDto> Rows { get; set; }   //rows

        public long TotalAllottedTime { get; set; }
        public long TotalAttendedTime { get; set; }

        public bool IsSingleMonth { get; set; }
        public int DaysInMonth1 { get; set; }
        public int DaysInMonth2 { get; set; }
        public string  Month1Text { get; set; }
        public string  Month2Text { get; set; }
        public string  Year1Text { get; set; }
        public string  Year2Text { get; set; }
        public int SiteWorkingTime { get; set; }

        public int TotalEmployeesCount { get; set; } = 0;
        public bool IsExistEmptyShifts { get; set; } = false;
    }


    public class Column_OpAttendanceReportForPayRollPeriodDto
    {
        public DateTime AttnDate { get; set; }
        public int  Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string DayText { get; set; }
        public bool IsHoliday { get; set; } = false;
        public HRM_DEF_HolidayMaster HolidayInf { get; set; } = new();

    }
     public class Row_OpAttendanceReportForPayRollPeriodDto
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameAr { get; set; }
        public string Position { get; set; }
        public string PositionName { get; set; }
        public string ShiftCode { get; set; }
        public List<AttendanceReportCellData> AttendanceMatrix { get; set; }
        public int UniqueShiftsCount { get; set; }
        public bool IsNewHire { get; set; } = false;
    }

    public class AttendanceReportCellData {
        //public Row_OpAttendanceReportForPayRollPeriodDto Row { get; set; }
        //public Column_OpAttendanceReportForPayRollPeriodDto Column { get; set; }
        public string AttnFlag { get; set; }
        public bool? IsOnLeave { get; set; }
        public bool? IsTransfered { get; set; }
        public bool? IsResigned { get; set; }
        public bool? IsTerminated { get; set; } = false;
        public bool? IsPosted { get; set; } = false;

        public long AttndId { get; set; }
        public long AltAttId { get; set; }
        public long RefIdForAlt { get; set; }
        public string AltEmployeeNumber { get; set; }
        public int AllottedShiftTime { get; set; }
        public int ContractualShiftTime { get; set; }
        public int AttendedTime { get; set; }

    }

    public class Input_OpAttendanceReportForPayRollPeriodDto
    {
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        
        public DateTime PayrollStartDate { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string EmployeeNumber { get; set; } = "";
    }

    #endregion
    #region attendanceStatusReport
    public class Input_OpAttendanceStatusReportForPayRollPeriodDto
    {
        public string BranchCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string AttendanceStatus { get; set; }
        public DateTime PayrollStartDate { get; set; }

        public int? PageNumber { get; set; } = 0;
        public int? PageSize { get; set; } = 0;


    }
    public class AttendanceStatusReportCellData
    {
        public bool? IsAttnDrafted { get; set; }
        public bool? IsAttnPosted { get; set; }

    }

    public class Row_OpAttendanceStatusReportForPayRollPeriodDto
    {
        public string BranchCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public List<AttendanceStatusReportCellData> AttendanceStatusMatrix { get; set; }
    }

    public class Column_OpAttendanceStatusReportForPayRollPeriodDto
    {
        public DateTime? AttnDate { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string DayText { get; set; }

    }
    public class Output_OpAttendanceStatusReportForPayRollPeriodDto
    {
        public bool IsValidReq { get; set; }
        public string ErrorMsg { get; set; }
        public TblErpSysCompanyDto Company { get; set; }

        public List<Column_OpAttendanceStatusReportForPayRollPeriodDto> Columns { get; set; }   //columns
        public List<Row_OpAttendanceStatusReportForPayRollPeriodDto> Rows { get; set; }   //rows

        
        public bool IsSingleMonth { get; set; }
        public int DaysInMonth1 { get; set; }
        public int DaysInMonth2 { get; set; }
        public string Month1Text { get; set; }
        public string Month2Text { get; set; }
        public string Year1Text { get; set; }
        public string Year2Text { get; set; }

        public int TotalItemsCount { get; set; } = 0;
    }
    #endregion


   public class Output_OpRoasterReportForPayRollPeriodDto
    {
        public bool? IsValidReq { get; set; } = false;
        public string ErrorMsg { get; set; }

        public string EmployeeNumber { get; set; }
        public string Position { get; set; }
        public string PositionName { get; set; }
        public int Qty { get; set; } = 0;
        public decimal DailyHrs { get; set; } = 0;
        public decimal MonthlyHrs { get; set; } = 0;
        public decimal MonthlyShifts { get; set; } = 0;
        public decimal MonthlyOffs { get; set; } = 0;
        public List<Output_OpRoasterReportForPayRollPeriodDto> RowsByPositions { get; set; } = new();
        public List<Output_OpRoasterReportForPayRollPeriodDto> Rows { get; set; } = new();
    }
    }