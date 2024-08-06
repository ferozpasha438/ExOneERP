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
    [AutoMap(typeof(TblOpPvTransferWithReplacementReq))]
    public class TblOpPvTransferWithReplacementReqDto
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string SrcCustomerCode { get; set; }
        [StringLength(20)]
        public string SrcSiteCode { get; set; }
        [StringLength(20)]
        public string SrcProjectCode { get; set; }
         [StringLength(20)]
        public string DestCustomerCode { get; set; }
        [StringLength(20)]
        public string DestSiteCode { get; set; }
        [StringLength(20)]
        public string DestProjectCode { get; set; }
       

        [StringLength(20)]
        public string SrcEmployeeNumber { get; set; }
         [StringLength(20)]
        public string DestEmployeeNumber { get; set; }

        public DateTime? FromDate { get; set; }
        public bool IsApproved { get; set; }


        public bool IsMerged { get; set; }


        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int ApprovedBy { get; set; }
        public string FileUrl { get; set; }
        public int? FileUploadBy { get; set; }


    }
    public class TblOpPvTransferWithReplacementReqsPaginationDto : TblOpPvTransferWithReplacementReqDto
    {
        public string SrcProjectName { get; set; }
        public string DestProjectName { get; set; }
        public string SrcProjectNameAr { get; set; }
        public string DestProjectNameAr { get; set; }
        public string SrcSiteName { get; set; }
        public string DestSiteName { get; set; }
        public string SrcSiteNameAr { get; set; }
        public string DestSiteNameAr { get; set; }




        public bool CanEditReq { get; set; }
        public bool CanApproveReq { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFileUploadRequired { get; set; } = false;

    }



}
 
