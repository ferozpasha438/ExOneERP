using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN.Domain;

namespace CIN.Domain.SchoolMgt
{
    [Table("tblStudentHomeWork")]
    public class TblStudentHomeWork
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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

    [Table("tblLessonPlanHeader")]
    public class TblLessonPlanHeader
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string LessonPlanCode { get; set; }
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [StringLength(200)]
        public string GradeCode { get; set; }
        [StringLength(200)]
        public string BranchCode { get; set; }
        [StringLength(200)]
        public string SectionCode { get; set; }
        public string SubCodes { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public int NumOfLessons { get; set; }
        public int NumOfDays { get; set; }
    }
    [Table("tblLessonPlanDetails")]
    public class TblLessonPlanDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
 


    [Table("tblDefSchoolTeacherMaster")]
    public class TblDefSchoolTeacherMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [StringLength(20)]
        public string TeacherCode { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherShortName { get; set; }
        [Required]
        [StringLength(250)]
        public string TeacherName1 { get; set; }
        [Required]
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
        [Required]
        public int SysLoginId { get; set; }
        public string AboutTeacher { get; set; }

        public string TeacherEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        [StringLength(250)]
        public string CreatedBy { get; set; }
    }

    [Table("tblDefSchoolTeacherLanguages")]
    public class TblDefSchoolTeacherLanguages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [Table("tblDefSchoolTeacherQualification")]
    public class TblDefSchoolTeacherQualification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [Table("tblDefSchoolTeacherSubjectsMapping")]
    public class TblDefSchoolTeacherSubjectsMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [Table("tblDefSchoolTeacherClassMapping")]
    public class TblDefSchoolTeacherClassMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TeacherCode { get; set; }
        [Required]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SectionCode { get; set; }
        public bool IsMapped { get; set; }
    }

    [Table("tblDefSchoolSubjectExamsGrade")]
    public class TblDefSchoolSubjectExamsGrade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        [Required]
        [StringLength(200)]
        public string SubjectCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaximumMarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumMarks { get; set; }
        public string QualifiyingGrade { get; set; }
    }

    [Table("tblDefSchoolExaminationManagementHeader")]
    public class TblDefSchoolExaminationManagementHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    [Table("tblDefSchoolExaminationManagementDetails")]
    public class TblDefSchoolExaminationManagementDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ExamHeaderId { get; set; }
        [Required]
        [StringLength(200)]
        public string SubjectCode { get; set; }
        public DateTime StartingDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
    }

    [Table("tblDefSchoolStudentResultHeader")]
    public class TblDefSchoolStudentResultHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ExamId { get; set; }
        [Required]
        [StringLength(200)]
        public string GradeCode { get; set; }
        public bool IsRelease { get; set; }
        public string ReleasedBy { get; set; }
        public DateTime ResultDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    [Table("tblDefSchoolStudentResultDetails")]
    public class TblDefSchoolStudentResultDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StudentResultHeaderId { get; set; }
        [Required]
        [StringLength(200)]
        public string StudentAdmNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string SubCodes { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaximumMarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal QualifiyingMarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MarksObtained { get; set; }
        public string QualifiyingGrade { get; set; }
        [StringLength(2000)]
        public string Remarks { get; set; }
    }


    [Table("TblStudentAttnRegHeader")]
    public class TblStudentAttnRegHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [Table("tblStudentAttnRegDetails")]
    public class TblStudentAttnRegDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AttnRegHeaderId { get; set; }
        [Required]
        [StringLength(200)]
        public string StudentAdmNumber { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public Char AttnFlag { get; set; }
        public string Remarks { get; set; }
        public bool IsPresent { get; set; }
        public bool IsLeave { get; set; }
    }

   



}
