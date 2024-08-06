using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application
{
    [AutoMap(typeof(TblOpPvOpenCloseReq))]
    public class TblOpPvOpenCloseReqDto
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }

        public bool IsSuspendReq { get; set; }
        public bool IsCancelReq { get; set; }
        public bool IsCloseReq { get; set; }
        public bool IsReOpenReq { get; set; }
        public bool IsRevokeSuspReq { get; set; }
        public bool IsExtendProjReq { get; set; }


        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExtensionDate { get; set; }
        



        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int ModifiedBy { get; set; }
        public bool IsApproved { get; set; }
       

        public int ApprovedBy { get; set; }
        public string FileUrl { get; set; }
        public int? FileUploadBy { get; set; }

    }
     public class TblOpPvOpenCloseReqsPaginationDto : TblOpPvOpenCloseReqDto
    {
        public string ProjectName { get; set; }
        public string ProjectNameAr { get; set; }
        public string SiteName { get; set; }
        public string SiteNameAr { get; set; }
     
        public string ReqType { get; set; }

        public bool CanEditReq { get; set; }
        public bool CanApproveReq { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFileUploadRequired { get; set; } = true;
        public string RequestType { get; set; } = "ProjectVariations";
        public string RequestSubType { get; set; }
        public long RequestNumber { get; set; }
    }



}
 
