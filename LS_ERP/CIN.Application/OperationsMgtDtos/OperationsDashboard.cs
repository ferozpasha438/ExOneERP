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

    public class OperationsDashboardDto
    {

        public int EnquiriesCurentMonth { get; set; }
        public int EnquiriesCurentYear { get; set; }
        public int NoOfCustomers { get; set; }
        public int NoOfSites { get; set; }
        public int NoOfUnAppSur { get; set; }
        public int NoOfAppSur { get; set; }
        public int NoOfUnAppEst { get; set; }
        public int NoOfAppEst { get; set; }
        public int NoOfContracts { get; set; }
        public decimal ConversionCurcotContract { get; set; }
    } 
    public class OperationsManagementDashboardOpDto
    {

        
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftInTime { get; set; }
        public string ShiftOutTime { get; set; }
        public int TotalContracted { get; set; } = 0;
        public int StaffPresent { get; set; } = 0;
        public int LateStaff { get; set; } = 0;
        public int Shortage { get; set; } = 0;
        public int SupportGaurds { get; set; } = 0;
        public int ShiftsNotAssignedCount { get; set; } = 0;
        public int LeavesCount { get; set; } = 0;
        public int TotalItemsCount { get; set; } = 0;
        public bool ProjectStatus { get; set; } = true;
        public List<OperationsManagementDashboardOpDto> Rows { get; set; } = new();

        public List<CustomSelectListItem> ProjectsSelectionList { get; set; } = new();
        public List<CustomSelectListItem> SitesSelectionList { get; set; } = new();
    }

     public class OperationsDashboardOpDto
    {

        public string BranchCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public int ReportedEmpCount { get; set; } = 0;
        public int NotReportedEmpCount { get; set; } = 0;
        public int ShiftsNotAssignedCount { get; set; } = 0;
        public int TotalEmpCount { get; set; } = 0;
        public int SwapEmpCount { get; set; } = 0;
        public int LeavesCount { get; set; } = 0;
        public int WithdrawalsCount { get; set; } = 0;
        public int LateArrivalsCount { get; set; } = 0;

     
        public int TotalItemsCount { get; set; } = 0;
        public List<DashboardEmployeeAttendanceDto> EmployeeAttendance { get; set; } = new();

        public List<CustomSelectListItem> ProjectsSelectionList { get; set; } = new();
        public List<CustomSelectListItem> SitesSelectionList { get; set; } = new();
        public List<CustomSelectListItem> EmployeesSelectionList { get; set; } = new();
    }




    public class OperationsDashboardIpDto 
    {
        public string SortingOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string BranchCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string EmployeeNumber { get; set; }
        public string SkillsetCode { get; set; } = "SST000002";
        public DateTime? Date { get; set; }
        public string DashBoardSubType { get; set; } = "operations";

        public List<OperationsDashboardFilterOptionsDto> FilterOptions { get; set; } =new();

    }
    
    public class OperationsDashboardFilterOptionsDto
    {
        public string Key { get; set; }
        public bool IsSelected { get; set; }
    }
}
