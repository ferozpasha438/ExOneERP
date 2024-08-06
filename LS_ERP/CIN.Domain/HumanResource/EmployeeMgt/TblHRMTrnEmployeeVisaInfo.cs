using CIN.Domain.HumanResource.Setup;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeVisaInfo")]
    public class TblHRMTrnEmployeeVisaInfo : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Country
        [ForeignKey(nameof(CountryCode))]
        public TblErpSysCountryCode SysCountryCode { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        //Visa Type
        [ForeignKey(nameof(VisaTypeCode))]
        public TblHRMSysVisaType SysVisaType { get; set; }
        [Required]
        [StringLength(20)]
        public string VisaTypeCode { get; set; }

        //Visa Number
        [Required]
        [StringLength(30)]
        public string VisaNumber { get; set; }

        //Valid From
        [Required]
        [Column(TypeName = "date")]
        public DateTime ValidFrom { get; set; }

        //Valid To
        [Required]
        [Column(TypeName = "date")]
        public DateTime ValidTo { get; set; }

        //Issue Location
        [StringLength(30)]
        public string IssueLocation { get; set; }
    }
}
