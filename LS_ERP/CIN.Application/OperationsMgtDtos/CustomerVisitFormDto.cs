using AutoMapper;
using CIN.Application;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{

    [AutoMap(typeof(TblOpCustomerVisitForm))]
    public class TblOpCustomerVisitFormDto 
    {
        public int Id { get; set; }

        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }

        public string SiteCode { get; set; }
        public string BranchCode { get; set; }

      
        public string ReasonCode { get; set; }
        public string SupervisorRemarks { get; set; }
        public string CustomerRemarks { get; set; }
        public string ActionTerms { get; set; }
        public string CustomerNotes { get; set; }
        public string ContactNumber { get; set; }
        
        public int SupervisorId { get; set; }               //Id in Login Table
        public int? VisitedBy { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? ScheduleDateTime { get; set; }
        public DateTime? VisitedDateTime { get; set; }

    
        public DateTime? ModifiedOn { get; set; }
     
        public DateTime? CreatedOn { get; set; }
        public bool? IsOpen { get; set; } = true;
        public bool? IsClosed { get; set; } = false;
        public bool? IsInprogress { get; set; } = false;
    }


    public class InputTblOpCustomerVisitFormDto : TblOpCustomerVisitFormDto
    {
        public string Action { get; set; }    //new,edit,confirm,visit
    } 
        public class TblOpCustomerVisitFormPaginatioDto : TblOpCustomerVisitFormDto
    {
        public string CustomerNameEng { get; set; }
        public string CustomerNameArb { get; set; }
        public string ProjectNameEng { get; set; }
        public string ProjectNameAr { get; set; }
        public string SiteNameEng { get; set; }
        public string SiteNameAr { get; set; }
        public string ReasonCodeNameEng { get; set; }
        public string ReasonCodeNameAr { get; set; }
        public string NameVisitedBy { get; set; }
        public string NameSupervisorId { get; set; }
        public string NameCreatedBy { get; set; }
        public string NameModifiedBy { get; set; }

        public bool CanEdit { get; set; }
        public bool IsCRM { get; set; }//Customer RelationshipManager

        public string   Status { get; set; }
    }

    public class GetCustomerVisitFormDto: TblOpCustomerVisitFormPaginatioDto
    {
        public string CustomerAddress { get; set; }
        public string SiteAddress { get; set; }
    }
}
