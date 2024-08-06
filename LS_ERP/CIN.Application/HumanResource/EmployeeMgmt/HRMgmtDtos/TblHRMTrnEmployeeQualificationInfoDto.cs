using AutoMapper;
using CIN.Domain.HumanResource.EmployeeMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeQualificationInfo))]
    public class TblHRMTrnEmployeeQualificationInfoDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Is Technical Qualification
        [Required]
        public bool IsTechnicalQualification { get; set; }

        //Type of Degree(Under Graduate, Graduate, Post Graduate or Doctoral Degree).
        [StringLength(20)]
        public string DegreeTypeCode { get; set; }

        //Qualification
        [StringLength(20)]
        [Required]
        public string QualificationCode { get; set; }

        //Course Type such as Part Time or Full Time.
        [StringLength(20)]
        public string CourseTypeCode { get; set; }

        //Date of Cerification
        [Required]
        public DateTime DateOfCertification { get; set; }

        //College or University
        [Required]
        [StringLength(200)]
        public string CollegeOrUniversity { get; set; }

        //Country
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }

        [StringLength(100)]
        public string DegreeTypeName { get; set; }

        [StringLength(100)]
        public string QualificationName { get; set; }
        
        [StringLength(100)]
        public string CourseTypeName { get; set; }

        [StringLength(100)]
        public string CountryName { get; set; }
    }
}
