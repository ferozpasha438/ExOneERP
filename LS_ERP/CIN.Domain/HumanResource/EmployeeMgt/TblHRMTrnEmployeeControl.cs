using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeControls")]

    public class TblHRMTrnEmployeeControls : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        //This flag is applicable only if 'IsRoasterApplicable' is true.
        //When 'IsShiftApplicable' is true, Employee will be working in different shift in the given week in HRM.
        //When 'IsShiftApplicable' is false, Employee will be working in Same shift in the given week in HRM.
        public bool IsShiftApplicable { get; set; }
        //If true, Roaster will be Generated on every PayrollPeriod in HRM Module.
        //else if false, only Shifts will applied to employee in HRM and Roaster will be generated from other module such as Operations Module.
        public bool IsRoasterApplicable { get; set; }
        public bool IsUser { get; set; }
        public int? UserId { get; set; }
    }
}
