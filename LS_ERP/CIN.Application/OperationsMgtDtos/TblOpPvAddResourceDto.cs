using AutoMapper;
using CIN.Application.OperationsMgtDtos;
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
    [AutoMap(typeof(TblOpPvAddResourceReqHead))]
    public class TblOpPvAddResourceReqHeadDto
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        public bool IsApproved { get; set; }
        public bool IsEmpMapped { get; set; }

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
 public class OpPvAddResourceReqHeadDto: TblOpPvAddResourceReqHeadDto
    {
        public decimal ToatalAmount { get; set; }
        public List<TblOpPVAddResourceDto> ResourceList { get; set; }
    }




    [AutoMap(typeof(TblOpPvAddResource))]
    public class TblOpPVAddResourceDto 
    {
       
        public long Id { get; set; }
        public long AddResReqHeadId { get; set; }

        public int Qty { get; set; }
        [StringLength(20)]
        public string SkillsetCode { get; set; }
      
        public decimal PricePerUnit { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }



        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

    }
 public class InputPvAddResourceReq: TblOpPvAddResourceReqHeadDto
    {
        public List<TblOpPVAddResourceDto> ResourceList { get; set; }

    }

 public class PvAddResourceReqPagination: OpPvAddResourceReqHeadDto
    {
        public string ProjectName { get; set; }
        public string ProjectNameAr { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameAr { get; set; }
        public string SiteName { get; set; }
        public string SiteNameAr { get; set; }

        public TblOpAuthorities Authorities { get; set; }



        public bool CanEditReq { get; set; }
        public bool CanApproveReq { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFileUploadRequired { get; set; } = true;
        public string  RequestType { get; set; } = "AddResource";
        public long  RequestNumber { get; set; }


    }
}
