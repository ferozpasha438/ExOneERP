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
    [Table("tblHRMTrnEmployeeQualificationInfo")]
    public class TblHRMTrnEmployeeQualificationInfo : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Is Technical Qualification
        [Required]
        public bool IsTechnicalQualification { get; set; }

        //Type of Degree(Under Graduate, Graduate, Post Graduate or Doctoral Degree).
        [ForeignKey(nameof(DegreeTypeCode))]
        public TblHRMSysDegreeType SysDegreeType { get; set; }
        [StringLength(20)]
        [Required]
        public string DegreeTypeCode { get; set; }

        //Qualification
        [ForeignKey(nameof(QualificationCode))]
        public TblHRMSysQualification SysQualification { get; set; }
        [StringLength(20)]
        [Required]
        public string QualificationCode { get; set; }

        //Course Type such as Part Time or Full Time.
        [ForeignKey(nameof(CourseTypeCode))]
        public TblHRMSysCourseType SysCourseType { get; set; }
        [StringLength(20)]
        public string CourseTypeCode { get; set; }

        //Date of Cerification
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfCertification { get; set; }

        //College or University
        [Required]
        [StringLength(200)]
        public string CollegeOrUniversity { get; set; }

        //Country
        [ForeignKey(nameof(CountryCode))]
        public TblErpSysCountryCode SysCountryCode { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
