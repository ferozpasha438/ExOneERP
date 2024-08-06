using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(OP_HRM_TEMP_Project))]
    public class OP_HRM_TEMP_ProjectDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [StringLength(20)]
        public string ProjectCode { get; set; }
        //[StringLength(20)]
        public string CustomerCode { get; set; }

        [StringLength(200)]
        public string ProjectNameEng { get; set; }
     
        [StringLength(200)]
        public string ProjectNameArb { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
       
        public DateTime? StartDate { get; set; }
    
        public DateTime? EndDate { get; set; }
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


        public int? FileUploadBy { get; set; }
        public string FileUrl { get; set; }
    }
    public class ConvertCustToProjectDto : OP_HRM_TEMP_ProjectDto
    {
        public string EnquiryNumber { get; set; }
       
    }
    public class OP_HRM_TEMP_Project_PaginationDto : OP_HRM_TEMP_ProjectDto
    {
        public TblOpAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool IsApproved { get; set; }                //Estimation Approved
        public bool HasAuthority { get; set; }
        public bool IsAdmin { get; set; }
    }
   
    public class InputFileUploadForProject: OP_HRM_TEMP_Project_PaginationDto
    {
        public IFormFile FileIForm { get; set; }
        public string FileName { get; set; }
        public string WebRoot { get; set; }


    }

}
