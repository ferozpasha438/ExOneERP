using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefServiceEnquiries))]
    public class TblSndDefServiceEnquiriesDto          // : AutoActiveGenerateIdAuditableKey<int>
    {
        public int EnquiryID { get; set; }
        public string EnquiryNumber { get; set; }
        public string SiteCode { get; set; }
        public string ServiceCode { get; set; }
        public string UnitCode { get; set; }

        public int UnitQuantity { get; set; }
        public int ServiceQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal EstimatedPrice { get; set; }
        public string SurveyorCode { get; set; }
        public string StatusEnquiry { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }




        public bool IsAssignedSurveyor { get; set; }
        public bool IsSurveyInProgress { get; set; }
        public bool IsSurveyCompleted { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSurveyApproved { get; set; }
    }

    public class ServiceEnquiriesPaginationDto : TblSndDefServiceEnquiriesDto
    {
        public bool CanEditSurveyForm { get; set; }
        public TblOpAuthorities Authorities { get; set; }
        public bool ApprovedUser { get; set; }
        public bool HasAuthority { get; set; }
        public bool IsEnqApproved { get; set; }


        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public string SiteNameEn { get; set; }
        public string SiteNameAr { get; set; }

    }
}