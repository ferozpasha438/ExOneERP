using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblOpProjectSites))]
    public class TblOpProjectSitesDto 
    {
        public int Id { get; set; }
    
        public DateTime? CreatedOn { get; set; }
     
        public DateTime? ModifiedOn { get; set; }

        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }

        [StringLength(200)]
        public string ProjectNameEng { get; set; }
     
        [StringLength(200)]
        public string ProjectNameArb { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
       
        public DateTime? StartDate { get; set; }
    
        public DateTime? EndDate { get; set; }

        public DateTime? ActualEndDate { get; set; }



        public bool IsResourcesAssigned { get; set; }
        public bool IsMaterialAssigned { get; set; }
        public bool IsLogisticsAssigned { get; set; }
        public bool IsShiftsAssigned { get; set; }
        public bool IsExpenceOverheadsAssigned { get; set; }
        public bool IsEstimationCompleted { get; set; }
        public bool IsSkillSetsMapped { get; set; }
        public bool IsConvertedToProposal { get; set; }
        public bool IsConvrtedToContract { get; set; }
        public string BranchCode { get; set; }

        public bool IsAdendum { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsClosed { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsInActive { get; set; }
        public bool IsActive { get; set; }

        public short? SiteWorkingHours { get; set; }


        public int? FileUploadBy { get; set; }
        public string FileUrl { get; set; }
        public bool CanExtendProject { get; set; } = false;

    }
   
    public class TblOpProjectSites_PaginationDto : TblOpProjectSitesDto
    {
        public DateTime OldestRoasterStartDate { get; set; }
        public TblOpAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool IsApproved { get; set; }            //EstimationFor Addendum approved
        public bool HasAuthority { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEstimationForProjectApproved { get; set; }
      }

     
    public class ConvertCustToProjectSitesDto 
    {
        public List<TblOpProjectSitesDto> ProjectSites { get; set; }
    }

    public class SkippEstimationTypeDto
    {
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string Type { get; set; }        //resource,logistics,material,financeExpence
    }
    public class InputUploadContractFormForProjectSite
    {
        public int Id { get; set; }
        public bool IsAdendum { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public IFormFile FileIForm { get; set; }
        public string FileName { get; set; }
        public string WebRoot { get; set; }

    }

    
}
