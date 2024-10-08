using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeContactInfo")]
    public class TblHRMTrnEmployeeContactInfo : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Primary Phone Number
        [Required]
        [StringLength(15)]
        public string PrimaryPhoneNumber { get; set; }

        //Alternate Phone Number
        [StringLength(15)]
        public string AlternatePhoneNumber { get; set; }

        //Email
        [StringLength(256)]
        public string Email { get; set; }

    }
}
