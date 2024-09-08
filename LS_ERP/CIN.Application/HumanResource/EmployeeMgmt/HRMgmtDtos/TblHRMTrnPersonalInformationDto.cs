using AutoMapper;
using CIN.Domain.HumanResource.EmployeeMgt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos
{
    [AutoMap(typeof(TblHRMTrnPersonalInformation))]
    public class TblHRMTrnPersonalInformationDto : AuditableEntityDto<int>
    {
        [Required]
        [StringLength(30)]
        public string PrimaryNumber { get; set; }
        [StringLength(30)]
        public string IDNumber1 { get; set; }
        [StringLength(30)]
        public string IDNumber2 { get; set; }
        [StringLength(5)]
        //[Required]
        public string EmployeeNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstNameEn { get; set; }
        [Required]
        [StringLength(50)]
        public string LastNameEn { get; set; }
        [StringLength(50)]
        public string FirstNameAr { get; set; }
        [StringLength(50)]
        public string LastNameAr { get; set; }
        [StringLength(30)]
        public string NickNameEn { get; set; }
        [StringLength(30)]
        public string NickNameAr { get; set; }
        [StringLength(100)]
        public string FatherNameEn { get; set; }
        [StringLength(100)]
        public string MotherNameEn { get; set; }
        [StringLength(100)]
        public string FatherNameAr { get; set; }
        [StringLength(100)]
        public string MotherNameAr { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(20)]
        public string ReligionCode { get; set; }
        [Required]
        [StringLength(20)]
        public string EmployeeTypeCode { get; set; }
        [Required]
        [StringLength(20)]
        public string BloodGroupCode { get; set; }
        [Required]
        [StringLength(20)]
        public string GenderCode { get; set; }
        [Required]
        [StringLength(20)]
        public string MaritalStatusCode { get; set; }
        [Required]
        [StringLength(20)]
        public string TitleCode { get; set; }
        [StringLength(20)]
        public string GroupCode { get; set; }
        [StringLength(20)]
        public string SubGroupCode { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public DateTime? MarriageDate { get; set; }
        public bool IsPhysicallyChallenged { get; set; }
        [StringLength(500)]
        public string PHDescription { get; set; }
        public List<TblHRMTrnEmployeeLanguageInfoDto> EmployeeLanguages { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string CountryName { get; set; }
        public string IsActiveStatus { get; set; }
        [StringLength(80)]
        public string EmployeeImageUrl { get; set; }
        public bool AllowImageUpload { get; set; }
        //Profile Image Name with Guid and file extension.
        [StringLength(80)]
        public string ProfileFileName { get; set; }
    }
}
