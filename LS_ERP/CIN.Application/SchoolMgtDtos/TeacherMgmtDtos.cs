using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CIN.Application.SchoolMgtDtos
{
    [AutoMap(typeof(TblStudentHomeWork))]
    public class TblStudentHomeWorkDto
    {
        public int Id { get; set; }
        public DateTime HomeworkDate { get; set; }
        public string GradeCode { get; set; }
        public string SubCodes { get; set; }
        public string TeacherCode { get; set; }
        public string HomeWorkDescription { get; set; }
        public string HomeWorkDescription_Ar { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    //TimeTable
    [AutoMap(typeof(TblLessonPlanHeader))]
    public class TblLessonPlanHeaderDto
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string LessonPlanCode { get; set; }
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [StringLength(200)]
        public string GradeCode { get; set; }
        [StringLength(200)]
        public string BranchCode { get; set; }
        [StringLength(200)]
        public string SubCodes { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public int NumOfLessons { get; set; }
        public int NumOfDays { get; set; }
    }

    [AutoMap(typeof(TblLessonPlanDetails))]
    public class TblLessonPlanDetailsDto
    {

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string LessonPlanCode { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SectionCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SubCodes { get; set; }
        [Required]
        [StringLength(250)]
        public string Chapter { get; set; }
        [Required]
        public string LessonName { get; set; }
        [Required]
        public string LessonName2 { get; set; }
        [Required]
        [StringLength(20)]
        public string NumOfSessions { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        [StringLength(20)]
        public string EstDays { get; set; }
        [StringLength(20)]
        public string EstHrs { get; set; }
        public DateTime? ActStartDate { get; set; }
        public DateTime? ActEndDate { get; set; }
        [StringLength(20)]
        public string ActHrs { get; set; }
        public string Topics { get; set; }
        public string Topics2 { get; set; }
        [StringLength(200)]
        public string AssignTeacherCode { get; set; }
        [StringLength(200)]
        public string ActualTecherCode { get; set; }


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
        public DateTime CreatedOn { get; set; }
        [StringLength(250)]
        public string CreatedBy { get; set; }
        public string TeacherEmail { get; set; }
    }

    public class SchoolTeachersListDto
    {
        public string TeacherCode { get; set; }
        public string TeacherName1 { get; set; }
        public string TeacherName2 { get; set; }
    }
    public class SysLoginModeratorDto
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
    }
    public class SchoolTeacherMasterDto
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
        public IFormFile ThumbNailimageFileName { get; set; }
        [StringLength(500)]
        public string FullImageParh { get; set; }
        public IFormFile FullImageFileName { get; set; }
        [StringLength(250)]
        public string PrimaryBranchCode { get; set; }
        public string AboutTeacher { get; set; }
        public int SysLoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TeacherEmail { get; set; }
    }

    [AutoMap(typeof(TblDefSchoolTeacherLanguages))]
    public class TblDefSchoolTeacherLanguagesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [Required]
        [StringLength(200)]
        public string LanguageCode { get; set; }
        [Required]
        public int Read { get; set; }
        [Required]
        public int Write { get; set; }
        [Required]
        public int Speak { get; set; }
    }
    public class SchoolTeacherLanguagesListDto
    {
        public List<TblDefSchoolTeacherLanguagesDto> TeacherLanguagesList { get; set; } = new List<TblDefSchoolTeacherLanguagesDto>();
    }
    [AutoMap(typeof(TblDefSchoolTeacherQualification))]
    public class TblDefSchoolTeacherQualificationDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [Required]
        public string Qualification { get; set; }
        [Required]
        public string Institute { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Grade { get; set; }
        [Required]
        public string Percentage { get; set; }
    }
    public class SchoolTeacherQualificationsListDto
    {
        public List<TblDefSchoolTeacherQualificationDto> TeacherQualificationsList { get; set; } = new List<TblDefSchoolTeacherQualificationDto>();
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
    public class SchoolTeacherSubjectsMappingListDto
    {
        public List<TblDefSchoolTeacherSubjectsMappingDto> TeacherSubjectsMappingList { get; set; } = new List<TblDefSchoolTeacherSubjectsMappingDto>();
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
        [Required]
        public string SectionCode { get; set; }
        public bool IsMapped { get; set; }
    }
    public class SchoolTeacherClassesMappingListDto
    {
        public List<TblDefSchoolTeacherClassMappingDto> TeacherClassesMappingList { get; set; } = new List<TblDefSchoolTeacherClassMappingDto>();
    }

    [AutoMap(typeof(TblStudentAttnRegHeader))]
    public class TblStudentAttnRegHeaderDto
    {
        public int Id { get; set; }
        public DateTime AttnDate { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [Required]
        [StringLength(200)]
        public string BranchCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SectionCode { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        public bool IsOpen { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    [AutoMap(typeof(TblStudentAttnRegDetails))]
    public class TblStudentAttnRegDetailsDto
    {
        public int Id { get; set; }
        public int AttnRegHeaderId { get; set; }
        [Required]
        [StringLength(200)]
        public string StudentAdmNumber { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public Char AttnFlag { get; set; }
        public string Remarks { get; set; }
        public bool IsPresent { get; set; }
        public bool IsLeave { get; set; }
    }

    [AutoMap(typeof(TblDefSchoolExaminationManagementHeader))]
    public class TblDefSchoolExamMgtHeaderDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string BranchCode { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string TypeOfExaminationCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public int PreparedBy { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public bool IsResultDeclared { get; set; }
        public DateTime? DateOfResult { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [AutoMap(typeof(TblDefSchoolExaminationManagementDetails))]
    public class TblDefSchoolExamMgtDetailsDto
    {
        public int Id { get; set; }
        public int ExamHeaderId { get; set; }
        [Required]
        [StringLength(200)]
        public string SubjectCode { get; set; }
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
    }

    public class SchoolExamMgtDto
    {
        public SchoolExamMgtDto()
        {
            SchoolExamMgtDetails = new List<TblDefSchoolExamMgtDetailsDto>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string BranchCode { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string TypeOfExaminationCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public int PreparedBy { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public bool IsResultDeclared { get; set; }
        public DateTime? DateOfResult { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<TblDefSchoolExamMgtDetailsDto> SchoolExamMgtDetails { get; set; }
    }

    public class SchoolExamManagementDto
    {
        public SchoolExamManagementDto()
        {
            TableRows = new List<SchoolExamManagementDetailsDto>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string BranchCode { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string TypeofExamination { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Remarks { get; set; }
        public int PreparedBy { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public bool IsResultDeclared { get; set; }
        public DateTime? DateofResult { get; set; }
        public List<SchoolExamManagementDetailsDto> TableRows { get; set; }
    }
    public class SchoolExamManagementDetailsDto
    {
        [Required]
        [StringLength(200)]
        public string SubjectCode { get; set; }
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
    }
    public class ParametersForExamsDto
    {
        public ParametersForExamsDto()
        {
            TableRows = new();
        }
        public string GradeCode { get; set; }
        public int AcademicYear { get; set; }
        public bool IsGradeRequired { get; set; }
        public int NoOfGrades { get; set; }
        public List<SubjectGradesConfig> TableRows { get; set; }
    }
    public class SubjectGradesConfig
    {
        public SubjectGradesConfig()
        {
            ConfigRows = new();
        }
        public string SubCodes { get; set; }
        public string MaximumMarks { get; set; }
        public string QualifyingMarks { get; set; }
        public List<GradeConfig> ConfigRows { get; set; }
    }
    public class GradeConfig
    {
        public string MaximumMarks { get; set; }
        public string MinimumMarks { get; set; }
        public string QualifiyingGrade { get; set; }
    }
    public class ParametersForAcadamicGradeListDto
    {
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public string GradeName2 { get; set; }
        public bool IsGradeRequired { get; set; }
        public int NoOfGrades { get; set; }
    }
    public class StudentResultData
    {
        public string SubCodes { get; set; }
        public string SubjectName { get; set; }
        public string SubjectName2 { get; set; }
        public decimal? MaximumMarks { get; set; }
        public decimal? QualifyingMarks { get; set; }
        public decimal? SubjectMarks { get; set; }
        public string QualifyingGrade { get; set; }
    }
    public class StudentExamResultDataDto
    {
        public StudentExamResultDataDto()
        {
            StudentResults = new List<StudentResultData>();
        }
        public string StudentName { get; set; }
        public string StudentName2 { get; set; }
        public string StuAdmCode { get; set; }
        public string Remarks { get; set; }
        public decimal? TotalMarks { get; set; }
        public List<StudentResultData> StudentResults { get; set; }
    }
    public class StudentExamResultListDto
    {
        public StudentExamResultListDto()
        {
            StudentExamResultData = new List<StudentExamResultDataDto>();
        }
        public List<StudentExamResultDataDto> StudentExamResultData { get; set; }
        public int ExamHeaderID { get; set; }
        public string GradeCode { get; set; }
        public string BranchCode { get; set; }
    }


    public class StudentAttendanceRegisterDto
    {
        public int Id { get; set; }
        public DateTime AttnDate { get; set; }
        public string TeacherCode { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public bool IsOpen { get; set; }
        public List<StudentAttendanceDataDto> StudentAttendanceDataList { get; set; }
    }
    public class StudentAttendanceDataDto
    {
        public string StudentName { get; set; }
        public string StudentName2 { get; set; }
        public string StudentAdmNumber { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public Char AttnFlag { get; set; }
        public bool IsPresent { get; set; }
        public bool IsLeave { get; set; }
        public string Remarks { get; set; }
    }
    // for Mobile Using DTO


    public class MobStudentResultData
    {
        public string SubCodes { get; set; }
        public string SubjectName { get; set; }
        public string SubjectName2 { get; set; }
        public decimal? MaximumMarks { get; set; }
        public decimal? QualifyingMarks { get; set; }
        public decimal? SubjectMarks { get; set; }
        public string QualifyingGrade { get; set; }
    }

    public class MobStudentExamResultDataDto
    {
        public MobStudentExamResultDataDto()
        {
            StudentResults = new List<MobStudentResultData>();
        }
        public int ExamHeaderID { get; set; }
        public List<MobStudentResultData> StudentResults { get; set; }

    }


    public class MobStudentExamResultListDto
    {
        public MobStudentExamResultListDto()
        {
            StudentExamResultData = new List<MobStudentExamResultDataDto>();
        }


        public string StudentName { get; set; }
        public string StudentName2 { get; set; }
        public string StuAdmCode { get; set; }
        public List<MobStudentExamResultDataDto> StudentExamResultData { get; set; }
        public string GradeCode { get; set; }
        public string BranchCode { get; set; }

        public decimal AttnPercentage { get; set; }
        public int Totaldays { get; set; }
        public int TotalPresent { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalLeave { get; set; }
        public int TotalAbsent { get; set; }
        public string Remarks { get; set; }
    }

    public class TeacherLessonPlanInfoDto
    {
        public TeacherLessonPlanInfoDto()
        {
            TableRows = new();
        }
        public int Id { get; set; }
        public string LessonPlanCode { get; set; }
        public string BranchCode { get; set; }
        public string TeacherCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string SubCodes { get; set; }
        public DateTime EstStartDate { get; set; }
        public DateTime EstEndDate { get; set; }
        public int NumOfDays { get; set; }
        public int NumOfLessons { get; set; }
        public List<LessonPlanDetailsDto> TableRows { get; set; }

    }
    public class LessonPlanDetailsDto
    {
        public string Chapter { get; set; }
        public string LessonName { get; set; }
        public string LessonName2 { get; set; }
        public string Topics { get; set; }
        public string Topics2 { get; set; }
        public string NumOfSessions { get; set; }
        public DateTime TopicDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
