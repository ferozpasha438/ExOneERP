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
    [AutoMap(typeof(TblOpPvRemoveResourceReq))]
    public class TblOpPvRemoveResourceReqDto
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }


        [StringLength(20)]
        public string EmployeeNumber { get; set; }

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
    public class TblOpPvRemoveResourceReqsPaginationDto : TblOpPvRemoveResourceReqDto
    {

        public string ProjectName { get; set; }
        public string ProjectNameAr { get; set; }
        public string SiteName { get; set; }
        public string SiteNameAr { get; set; }




        public bool CanEditReq { get; set; }
        public bool CanApproveReq { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsFileUploadRequired { get; set; } = true;
        public string RequestType { get; set; } = "RemoveResource";
        public long RequestNumber { get; set; }
    }



}

















