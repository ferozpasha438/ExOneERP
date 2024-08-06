using AutoMapper;
using CIN.Application;
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

namespace CIN.Application.OperationsMgtDtos
{

    [AutoMap(typeof(TblOpCustomerComplaint))]
    public class TblOpCustomerComplaintDto
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }

        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }

       
        [StringLength(20)]
        public string ReasonCode { get; set; }


        public string ComplaintBy { get; set; }

        public string ComplaintDescription { get; set; }

        public string? ProofForComplaint { get; set; }    //Document Path
        public string? ProofForAction { get; set; }         //Document Path

        public string? ActionDescription { get; set; }

        public bool IsActionRequired { get; set; } = false;

        public DateTime ComplaintDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        public int BookedBy { get; set; }
        public int? CreatedBy { get; set; }
        public int? ClosedBy { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsOpen { get; set; } = true;
        public bool IsClosed { get; set; } = false;
        public bool IsInprogress { get; set; } = false;
    }


    public class InputTblOpCustomerComplaintDto : TblOpCustomerComplaintDto
    {
        //public int Id { get; set; }
        //[StringLength(20)]
        //public string CustomerCode { get; set; }
        //[StringLength(20)]
        //public string ProjectCode { get; set; }

        //[StringLength(20)]
        //public string SiteCode { get; set; }
        //[StringLength(20)]
        //public string BranchCode { get; set; }


        //[StringLength(20)]
        //public string ReasonCode { get; set; }


        //public string ComplaintBy { get; set; }

        //public string ComplaintDescription { get; set; }

        //public string ProofForComplaint { get; set; }    //Document Path
        //public string ProofForAction { get; set; }         //Document Path

        //public string ActionDescription { get; set; }

        //public bool IsActionRequired { get; set; } = false;

        //public DateTime ComplaintDate { get; set; }
        //public DateTime? ClosingDate { get; set; }

        //public int BookedBy { get; set; }

        //public bool IsOpen { get; set; } = true;
        //public bool IsClosed { get; set; } = false;
        //public bool IsInprogress { get; set; } = false;


        public string Action { get; set; }    //new,edit,confirm,closing

        public string ProofForComplaintFileName { get; set; }
        public string ProofForActionFileName { get; set; }

        public IFormFile ProofForComplaintIForm { get; set; }
        public IFormFile ProofForActionIForm { get; set; }


        public string WebRootForComplaints { get; set; }
        public string WebRootForActions { get; set; }
        // public List<TblOpFileUploadDto> Proofs { get; set; }

    } 
        public class TblOpCustomerComplaintsPaginationDto : TblOpCustomerComplaintDto
    {
        public string CustomerNameEng { get; set; }
        public string CustomerNameArb { get; set; }
        public string ProjectNameEng { get; set; }
        public string ProjectNameAr { get; set; }
        public string SiteNameEng { get; set; }
        public string SiteNameAr { get; set; }
        public string ReasonCodeNameEng { get; set; }
        public string ReasonCodeNameAr { get; set; }
       
        public string NameCreatedBy { get; set; }
        public string NameModifiedBy { get; set; }

        public string NameBookedBy { get; set; }
        public string NameClosedBy { get; set; }

        public bool CanEdit { get; set; }
        public bool IsCRM { get; set; }//Customer RelationshipManager

        public string   Status { get; set; }
    }

    public class GetCustomerComplaintDto: TblOpCustomerComplaintsPaginationDto
    {
        public string CustomerAddress { get; set; }
        public string SiteAddress { get; set; }
    }
}
