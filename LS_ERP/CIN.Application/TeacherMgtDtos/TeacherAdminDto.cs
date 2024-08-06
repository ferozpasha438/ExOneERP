using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.TeacherMgtDtos
{
    public class CheckCINTeacherDto
    {
        [Required(ErrorMessage = "*")]
        public string CINNumber { get; set; }
    }

    public class TeacherLoginSuccessMessageDto
    {
        public string Message { get; set; }
        public string Connectionstring { get; set; }
        public string BaseUrl { get; set; }
        public string ParentbaseUrl { get; set; }
        public bool Status { get; set; }

    }

    public class CheckTeacherLoginMetaDataDto
    {
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }


    public class ValidateCinFailedMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }


    }

    public class TeacherLoginFailedMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }


    }

    [AutoMap(typeof(TblDefSchoolTeacherMaster))]
   public class TblDefSchoolTeacherMasterDto
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string TeacherCode { get; set; }
        [StringLength(200)]
        public string TeacherShortName { get; set; }
        [StringLength(250)]
        public string TeacherName1 { get; set; }
        [StringLength(250)]
        public string TeacherName2 { get; set; }
        [StringLength(250)]
        public string FatherName { get; set; }
        [StringLength(50)]
        public string MaritalStatus { get; set; }
        [StringLength(250)]
        public string SpouseName { get; set; }
        [StringLength(250)]
        public string PAddress { get; set; }
        [StringLength(150)]
        public string Pcity { get; set; }
        [StringLength(20)]
        public string PPhone1 { get; set; }
        [StringLength(20)]
        public string PMobile1 { get; set; }
        [StringLength(250)]
        public string Saddress { get; set; }
        [StringLength(20)]
        public string SPhone2 { get; set; }
        [StringLength(20)]
        public string SMobile2 { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        public DateTime DateJoin { get; set; }
        [StringLength(50)]
        public string NationalityCode { get; set; }
        [StringLength(50)]
        public string NationalityID { get; set; }
        [StringLength(150)]
        public string Passport { get; set; }
        [StringLength(100)]
        public string HiringType { get; set; }
        [StringLength(100)]
        public string TotalExperience { get; set; }
        [StringLength(150)]
        public string HighestQualification { get; set; }
        [StringLength(150)]
        public string TechnologyCompetence { get; set; }
        [StringLength(150)]
        public string ComminicationSkills { get; set; }
        [StringLength(150)]
        public string TeachingSkills { get; set; }
        [StringLength(150)]
        public string Subjectknowledge { get; set; }
        [StringLength(150)]
        public string AdministrativeSkills { get; set; }
        [StringLength(150)]
        public string DisciplineSkills { get; set; }
        [StringLength(500)]
        public string ThumbNailImagePath { get; set; }
        [StringLength(500)]
        public string FullImageParh { get; set; }
        [StringLength(250)]
        public string PrimaryBranchCode { get; set; }
        public string AboutTeacher { get; set; }
        [Required]
        public int SysLoginId { get; set; }
        public string TeacherEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        [StringLength(250)]
        public string CreatedBy { get; set; }

 //       public string Message { get; set; }
 //       public bool Status { get; set; }

     //   public List<TblDefSchoolTeacherClassMappingDto> TeacherMapGradeList { get; set; }
     //   public List<TblDefSchoolTeacherSubjectsMappingDto> TeacherMapSubjectList { get; set; }
    }


    public class TeacherMasterLoginDto
    {
       
       public  TblDefSchoolTeacherMasterDto SchoolTeacherMaster { get; set; }
       public List<TblDefSchoolTeacherClassMappingDto> TeacherMapGradeList { get; set; }
       public List<TblDefSchoolTeacherSubjectsMappingDto> TeacherMapSubject { get; set; }

       public string Message { get; set; }
       public bool Status { get; set; }
    }

    [AutoMap(typeof(TblDefSchoolTeacherClassMapping))]
    public class TblDefSchoolTeacherClassMappingDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [Required]
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public bool IsMapped { get; set; }
    }

    [AutoMap(typeof(TblDefSchoolTeacherSubjectsMapping))]
    public class TblDefSchoolTeacherSubjectsMappingDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherCode { get; set; }
        public string GradeCode { get; set; }
        [Required]
        public string SubjectCode { get; set; }
        [Required]
        public int TeachingSkillLevel { get; set; }
        [Required]
        public int AdminSkillLevel { get; set; }
    }

    public class TeacherLoginMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        

    }


    public class ForgotPasswordDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }

    }

    public class CheckPasswordDto
    {
        [Required(ErrorMessage = "*")]
        public string TeacherCode { get; set; }
    }
}
