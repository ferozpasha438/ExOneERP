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
    [Table("tblHRMTrnPersonalInformation")]
    public class TblHRMTrnPersonalInformation : AuditableEntity<int>
    {
        [Required]
        [StringLength(30)]
        public string PrimaryNumber { get; set; }
        [StringLength(30)]
        public string IDNumber1 { get; set; }
        [StringLength(30)]
        public string IDNumber2 { get; set; }
        [StringLength(5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
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
        //Country
        [ForeignKey(nameof(CountryCode))]
        public TblErpSysCountryCode SysCountry { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }
        //Religion
        [ForeignKey(nameof(ReligionCode))]
        public TblHRMSysReligion SysReligion { get; set; }
        [Required]
        [StringLength(20)]
        public string ReligionCode { get; set; }
        //Employee Type
        [ForeignKey(nameof(EmployeeTypeCode))]
        public TblHRMSysEmployeeType SysEmployeeType { get; set; }
        [Required]
        [StringLength(20)]
        public string EmployeeTypeCode { get; set; }
        //Blood Group
        [ForeignKey(nameof(BloodGroupCode))]
        public TblHRMSysBloodGroup SysBloodGroup { get; set; }
        [Required]
        [StringLength(20)]
        public string BloodGroupCode { get; set; }
        //Gender
        [ForeignKey(nameof(GenderCode))]
        public TblHRMSysGender SysGender { get; set; }
        [Required]
        [StringLength(20)]
        public string GenderCode { get; set; }
        //Marital Status
        [ForeignKey(nameof(MaritalStatusCode))]
        public TblHRMSysMaritalStatus SysMaritalStatus { get; set; }
        [Required]
        [StringLength(20)]
        public string MaritalStatusCode { get; set; }
        //Title
        [ForeignKey(nameof(TitleCode))]
        public TblHRMSysTitle SysTitle { get; set; }
        [Required]
        [StringLength(20)]
        public string TitleCode { get; set; }
        //Group
        [ForeignKey(nameof(GroupCode))]
        public TblHRMSysGroup SysGroup { get; set; }
        [StringLength(20)]
        public string GroupCode { get; set; }
        //SubGroup
        [ForeignKey(nameof(SubGroupCode))]
        public TblHRMSysSubGroup SysSubGroup { get; set; }
        [StringLength(20)]
        public string SubGroupCode { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public DateTime? MarriageDate { get; set; }
        public bool IsPhysicallyChallenged { get; set; }
        [StringLength(500)]
        public string PHDescription { get; set; }
        [StringLength(80)]
        public string EmployeeImageUrl { get; set; }
    }
}
