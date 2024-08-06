using CIN.Application.SystemSetupDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    #region ProjectsSitesReports
    public class ProjectSitesReportsInputDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CustomerCode { get; set; }
        public string CityCode { get; set; }                    //BranchCode
        public string ServiceCode { get; set; }
        public string StatusCode { get; set; }
        public string SiteCode { get; set; }
        
    }
    public class ProjectSitesReportsOutputDto
    {
        public List<TblOpProjectSitesDto> ProjectSites { get; set; }
        public TblErpSysCompanyDto Company { get; set; }
        public List<TblSndDefCustomerMasterDto> Customers{ get; set; }


    }
    public class ProjectSiteReport : TblOpProjectSitesDto
    {
        public string SiteNameEng { get; set; }
        public string SiteNameArb { get; set; }
        public string Status { get; set; }
        public decimal? EstimationCost { get; set; }
        public List<EmployeeDtoForReports> ResourcesList { get; set; }          //Employees with latest Attendance
        public List<OpSkillsetDto> SkillsetsList { get; set; }          //Skillsets Count
        public int TotalSkillSetsQuantity { get; set; }
    }

    #endregion

    #region CustomerComplaintsReports
    public class CustomerComplaintsReportsInputDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CustomerCode { get; set; }
        public string CityCode { get; set; }                    //BranchCode
        public string BranchCode { get; set; }                    //BranchCode
        public string StatusCode { get; set; }           //Open,Inprogress,Closed
        public string SiteCode { get; set; }
        public string ReasonCode { get; set; }
        public string ProjectCode { get; set; }
        public string BookedBy { get; set; }

    }
    
    public class CustomerComplaintsReportsOutputDto
    {
        public List<GetCustomerComplaintDto> Complaints { get; set; } = new();
        public TblErpSysCompanyDto Company { get; set; } = new();
        public string NameBookedBy { get; set; } = "";
        public string NameEngReasonCode { get; set; } = "";
        public string NameArbReasonCode { get; set; } = "";
    }


    #endregion
#region CustomerVisitsReports
    public class CustomerVisitsReportsInputDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CustomerCode { get; set; }
        public string CityCode { get; set; }                    //BranchCode
        public string BranchCode { get; set; }                    //BranchCode
        public string StatusCode { get; set; }           //Open,Inprogress,Closed
        public string SiteCode { get; set; }
        public string ReasonCode { get; set; }
        public string ProjectCode { get; set; }

        public string SupervisorId { get; set; }
        public string VisitedBy { get; set; }

    }
    
    public class CustomerVisitsReportsOutputDto
    {
        public List<GetCustomerVisitFormDto>Visits { get; set; }
        public TblErpSysCompanyDto Company { get; set; }
        public string NameSupervisor { get; set; } = "";
        public string NameVisitedBy { get; set; } = "";
        public string NameEngReasonCode { get; set; } = "";
        public string NameArbReasonCode { get; set; } = "";
    }


    #endregion




    }
