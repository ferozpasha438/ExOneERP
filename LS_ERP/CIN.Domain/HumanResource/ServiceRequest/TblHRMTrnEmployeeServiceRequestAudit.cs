using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest
{
    [Table("tblHRMTrnEmployeeServiceRequestAudit")]
    public class TblHRMTrnEmployeeServiceRequestAudit : PrimaryKey<int>
    {
        //EmployeeServiceRequestID
        [ForeignKey(nameof(EmployeeServiceRequestID))]
        public TblHRMTrnEmployeeServiceRequest TrnEmployeeServiceRequest { get; set; }
        [Required]
        public int EmployeeServiceRequestID { get; set; }

        //EntryDate
        [Required]
        public DateTime EntryDate { get; set; }

        //EntryBy
        [ForeignKey(nameof(EntryBy))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EntryBy { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }

        //ActionID(An enum with Cancel = 0, Save = 1,Submit = 2, Reject = 3, Approved = 4, Release = 5, Reporting = 6, Settlement=7, Relieve=8)
        [Required]
        public int ActionID { get; set; }

        //ServiceRequestProcessStageID(Increment by 1 at each step.)
        [Required]
        public int ServiceRequestProcessStageID { get; set; }
    }
}
