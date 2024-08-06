using AutoMapper;
using CIN.Application;
using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeServiceRequestAudit))]
    public class TblHRMTrnEmployeeServiceRequestAuditDto : PrimaryKeyDto<int>
    {
        //EmployeeServiceRequestID

        [Required]
        public int EmployeeServiceRequestID { get; set; }

        //EntryDate
        [Required]
        public DateTime EntryDate { get; set; }

        //EntryBy

        [Required]
        public int EntryBy { get; set; }
        public string EntryName { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }

        //ActionID(An enum with Cancel = 0, Save = 1,Submit = 2, Reject = 3, Approved = 4, Release = 5, Reporting = 6, Settlement=7, Relieve=8)
        [Required]
        public int ActionID { get; set; }
        public string ActionName { get; set; }

        //ServiceRequestProcessStageID(Increment by 1 at each step.)
        [Required]
        public int ServiceRequestProcessStageID { get; set; }
    }
}
