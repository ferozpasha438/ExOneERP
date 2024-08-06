using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application
{

    public class OpPvAllRequestsPaginationDto
    {
        public long Id { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string RequestedBy { get; set; }
        public long RequestNumber { get; set; }
        public string RequestType { get; set; }
        public string RequestSubType { get; set; }


        public bool IsApproved { get; set; }
        public string Reamrks { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool? IsMappedResources { get; set; }            //applicable only for Add resource so that ?(nullable)
        public bool? IsMerged { get; set; }            //applicable only for Add resource so that ?(nullable)





        public string ProjectName { get; set; }
        public string ProjectNameAr { get; set; }
        public string SiteName { get; set; }
        public string SiteNameAr { get; set; }

        public bool CanEditReq { get; set; }
        public bool CanApproveReq { get; set; }

        public bool IsAdmin { get; set; }

        public string FileUrl { get; set; }
        public bool IsFileUploadRequired { get; set; }


    }

    public class PvRequestsFileUploadDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; }
        public string RequestSubType { get; set; }
        public IFormFile FileIForm { get; set; }
        public string FileName { get; set; }
        public string WebRoot { get; set; }
    }

}