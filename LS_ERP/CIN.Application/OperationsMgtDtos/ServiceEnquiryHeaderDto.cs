using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefServiceEnquiryHeader))]
    public class TblSndDefServiceEnquiryHeaderDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        
        [StringLength(20)]
        public string EnquiryNumber { get; set; }
        
       
        public string CustomerCode { get; set; }
       
        public DateTime DateOfEnquiry { get; set; }
        
        public DateTime EstimateClosingDate { get; set; }
        [StringLength(200)]
        public string UserName { get; set; }
        public decimal TotalEstPrice { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(20)]
        public string StusEnquiryHead { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
        public bool IsConvertedToProject { get; set; }
        public bool IsProjectConvertedToContract { get; set; }
    }


    public class SurveyFormsPaginationDto : TblSndDefServiceEnquiryHeaderDto
    {

        public TblOpAuthorities Authorities { get; set; }
        
        public bool ApprovedUser { get; set; }
        public bool HasAuthority { get; set; }



        public bool IsApproved { get; set; }
        public bool IsAssignedSurveyor { get; set; }
        public bool IsSurveyInProgress { get; set; }
        public bool IsSurveyCompleted { get; set; }

        public bool IsEnqApproved { get; set; }

        public string CustomerNameEn { get; set; }
        public string CustomerNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public string BranchNameAr { get; set; }

        public bool IsAdmin { get; set; }
        public short? Version { get; set; }
    }
}
