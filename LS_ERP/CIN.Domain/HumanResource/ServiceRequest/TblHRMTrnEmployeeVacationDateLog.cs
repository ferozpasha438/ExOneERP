using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN.Domain.HumanResource.EmployeeMgt;

namespace CIN.Domain.HumanResource.ServiceRequest
{
    [Table("tblHRMTrnEmployeeVacationDateLog")]
    public class TblHRMTrnEmployeeVacationDateLog : AuditableEntity<int>
    {
        [ForeignKey(nameof(EmployeeServiceRequestID))]
        public TblHRMTrnEmployeeServiceRequest TrnEmployeeServiceRequest { get; set; }
        [Required]
        public int EmployeeServiceRequestID { get; set; }
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }

    }
}
