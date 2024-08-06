using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest
{
    [Table("tblHRMTrnEmployeeVacationServiceRequestLeaveDetails")]
    public class TblHRMTrnEmployeeVacationServiceRequestLeaveDetails : PrimaryKey<int>
    {
        //EmployeeServiceRequestID
        [ForeignKey(nameof(EmployeeServiceRequestID))]
        public TblHRMTrnEmployeeServiceRequest TrnEmployeeServiceRequest { get; set; }
        [Required]
        public int EmployeeServiceRequestID { get; set; }

        //LeaveTypeCode
        [ForeignKey(nameof(LeaveTypeCode))]
        public TblHRMSysLeaveType SysLeaveType { get; set; }
        [Required]
        [StringLength(20)]
        public string LeaveTypeCode { get; set; }

        //FromDate
        [Required]
        public DateTime FromDate { get; set; }

        //ToDate
        [Required]
        public DateTime ToDate { get; set; }

        //NoOfDays
        public int NoOfDays { get; set; }
    }
}
