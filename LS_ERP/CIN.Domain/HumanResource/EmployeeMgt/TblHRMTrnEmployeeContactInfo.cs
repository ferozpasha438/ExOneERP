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
        //EmployeeNumber
        [ForeignKey(nameof(EmployeeNumber))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        [StringLength(5)]
        public string EmployeeNumber { get; set; }

        //Primary Phone Number
        [Required]
        [StringLength(10)]
        public string PrimaryPhoneNumber { get; set; }

        //Alternate Phone Number
        [StringLength(10)]
        public string AlternatePhoneNumber { get; set; }

        //Email
        [Required]
        [StringLength(30)]
        public string Email { get; set; }

    }
}
