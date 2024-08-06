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
    [Table("tblSysSchoolSetup")]
    public class TblSysSchoolSetup
    {
        [StringLength(25)]
        [Required]
        public string OrgCode { get; set; }
        [StringLength(100)]
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string LogoPath { get; set; }
        [StringLength(200)]
        [Required]
        public string Address { get; set; }
        [StringLength(200)]
        [Required]
        public string City { get; set; }
        [StringLength(20)]
        [Required]
        public string Mobile { get; set; }
        [StringLength(20)]
        [Required]
        public string Phone { get; set; }
        [StringLength(200)]
        [Required]
        public string email { get; set; }
        public string website { get; set; }
        public string OrgTagLine { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }
        [Required]
        public DateTime AnVacStartDate { get; set; }
        [Required]
        public DateTime AnVacEndDate { get; set; }
        [Required]
        public int CurrentAcademicYear { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }

    [Table("tblSysSchoolBranches")]
    public class TblSysSchoolBranches
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string BranchCode { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string BranchNameAr { get; set; }
        public DateTime? StartAcademicDate { get; set; }
        public DateTime? EndAcademicDate { get; set; }
        public string FinBranch { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StartWeekDay { get; set; }
        public string CurrencyCode { get; set; }
        public string Website { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string NumberOfWeekDays { get; set; }
        public string WeekOff1 { get; set; }
        public string WeekOff2 { get; set; }
        [Required]
        public string GeoLat { get; set; }
        [Required]
        public string GeoLong { get; set; }
        public string BranchPrefix { get; set; }
        public int? NextStuNum { get; set; }
        public int? NextFeeVoucherNum { get; set; }
        public TimeSpan Default_InTime { get; set; }
        public TimeSpan Default_OutTime { get; set; }
        public string BranchNotification_Moderator { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }

    [Table("tblSysSchoolAcademicBatches")]
    public class TblSysSchoolAcademicBatches
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //Identity
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AcademicYear { get; set; }  //Primary Key
        [Required]
        public DateTime AcademicStartDate { get; set; }
        [Required]
        public DateTime AcademicEndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolAcademicsSubects")]
    public class TblSysSchoolAcademicsSubects
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }       //Identity
        [Key]
        public string SubCodes { get; set; }  //Primary Key
        [Required]
        public string SubName { get; set; }
        public string SubName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolSectionsSection")]
    public class TblSysSchoolSectionsSection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string SectionCode { get; set; }
        [Required]
        public string SectionName { get; set; }
        public string SectionName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolAcedemicClassGrade")]
    public class TblSysSchoolAcedemicClassGrade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string GradeCode { get; set; }
        [Required]
        public string GradeName { get; set; }
        public string GradeName2 { get; set; }
        public string FileName { get; set; }
        public bool? IsGradeRequired { get; set; }
        public int? NoOfGrades { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int? GradeOrder { get; set; }

    }

    [Table("tblSysSchoolGradeSectionMapping")]
    public class TblSysSchoolGradeSectionMapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string GradeCode { get; set; }
        public string FileName { get; set; }
        [Required]
        public string SectionCode { get; set; }
        [Required]
        public decimal MaxStrength { get; set; }
        [Required]
        public decimal MinStrength { get; set; }
        [Required]
        public decimal AvgStrength { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("TblSysSchoolSemister")]
    public class TblSysSchoolSemister
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string SemisterCode { get; set; }
        [Required]
        public string SemisterName { get; set; }
        public string SemisterName2 { get; set; }
        public DateTime SemisterStartDate { get; set; }
        public DateTime SemisterEndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolGradeSubjectMapping")]
    public class TblSysSchoolGradeSubjectMapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string GradeCode { get; set; }
        [Required]
        public string SemisterCode { get; set; }
        [Required]
        public string SubCodes { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaximumMarks { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? QualifyingMarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolPETCategory")]
    public class TblSysSchoolPETCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string PETCode { get; set; }
        [Required]
        public string PETName { get; set; }
        public string PETName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolNationality")]
    public class TblSysSchoolNationality
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string NatCode { get; set; }
        [Required]
        public string NatName { get; set; }
        public string NatName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolReligion")]
    public class TblSysSchoolReligion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string RegCode { get; set; }
        [Required]
        public string RegName { get; set; }
        public string RegName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolGender")]
    public class TblSysSchoolGender
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string GenderCode { get; set; }
        [Required]
        public string GenderName { get; set; }
        public string GenderName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolLanguages")]
    public class TblSysSchoolLanguages
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string LangCode { get; set; }
        [Required]
        public string LangName { get; set; }
        public string LangName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolFeeTerms")]
    public class TblSysSchoolFeeTerms
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string TermCode { get; set; }
        [Required]
        public string TermName { get; set; }
        public string TermName2 { get; set; }
        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }
        public DateTime FeeDueDate { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolFeeType")]
    public class TblSysSchoolFeeType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string FeeCode { get; set; }
        [Required]
        public string FeesName { get; set; }
        public string FeeName2 { get; set; }
        public decimal EstFeeAmount { get; set; }
        public decimal MinFeeAmount { get; set; }
        public decimal MaxFeeAmount { get; set; }
        public decimal IsDiscountable { get; set; }
        public decimal MaxDiscPer { get; set; }
        public bool TaxApplicable { get; set; }
        public string TaxCode { get; set; }
        public string Frequency { get; set; }
        public string FeeGLAccount { get; set; }
        public string FeeTaxAccount { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolFeeStructureHeader")]
    public class TblSysSchoolFeeStructureHeader
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string FeeStructCode { get; set; }

        [ForeignKey(nameof(GradeCode))]
        public TblSysSchoolAcedemicClassGrade SysSchoolAcedemicClassGrade { get; set; }
        public string GradeCode { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblSysSchoolBranches SysSchoolBranches { get; set; }
        public string BranchCode { get; set; }
        public string FeeStructName { get; set; }
        public string FeeStructName2 { get; set; }
        public bool ApplyLateFee { get; set; }

        [ForeignKey(nameof(LateFeeCode))]
        public TblSysSchoolFeeType SysSchoolFeeType { get; set; }
        public string LateFeeCode { get; set; }
        public decimal ActualFeePayable { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolFeeStructureDetails")]
    public class TblSysSchoolFeeStructureDetails
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(FeeStructCode))]
        public TblSysSchoolFeeStructureHeader SchoolFeeStructureHeader { get; set; }
        public string FeeStructCode { get; set; }

        [ForeignKey(nameof(TermCode))]
        public TblSysSchoolFeeTerms SysSchoolFeeTerms { get; set; }
        public string TermCode { get; set; }

        [ForeignKey(nameof(FeeCode))]
        public TblSysSchoolFeeType SysSchoolFeeType { get; set; }
        public string FeeCode { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal MaxDiscPer { get; set; }
        public decimal ActualFeeAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolPayTypes")]
    public class TblSysSchoolPayTypes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string PayCode { get; set; }
        public string PayName { get; set; }
        public string PaName2 { get; set; }
        public string GLAccount { get; set; }
        public string BranchCode { get; set; }
        public bool AllowOtherBranchUse { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolStuLeaveType")]
    public class TblSysSchoolStuLeaveType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string LeaveCode { get; set; }
        [Required]
        public string LeaveName { get; set; }
        public string LeaveName2 { get; set; }
        public int MaxLeavePerReq { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [Table("tblSysSchoolHolidayCalanderStudent")]
    public class TblSysSchoolHolidayCalanderStudent
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime HDate { get; set; }
        public string HName { get; set; }
        public string HName2 { get; set; }
        [StringLength(200)]
        public string BranchCode { get; set; }
    }



    [Table("tblSysSchoolExaminationTypes")]
    public class TblSysSchoolExaminationTypes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string TypeOfExaminationCode { get; set; }
        [StringLength(250)]
        public string ExaminationTypeName { get; set; }
        [StringLength(250)]
        public string ExaminationTypeName2 { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    [Table("tblSysSchoolSchedule")]
    public class TblSysSchoolSchedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string ScheduleCode { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public TimeSpan? Day1In { get; set; }
        public TimeSpan? Day1Out { get; set; }
        public TimeSpan? Day2In { get; set; }
        public TimeSpan? Day2Out { get; set; }
        public TimeSpan? Day3In { get; set; }
        public TimeSpan? Day3Out { get; set; }
        public TimeSpan? Day4In { get; set; }
        public TimeSpan? Day4Out { get; set; }
        public TimeSpan? Day5In { get; set; }
        public TimeSpan? Day5Out { get; set; }
        public TimeSpan? Day6In { get; set; }
        public TimeSpan? Day6Out { get; set; }
        public TimeSpan? Day7In { get; set; }
        public TimeSpan? Day7Out { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }


  
   [Table("tblSysSchoolBranchesAuthority")]
   public class TblSysSchoolBranchesAuthority
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string BranchCode { get; set; }
        public int Level { get; set; }

        public bool IsApproveLeave { get; set; }

        public bool IsApproveDisciPlinaryAction { get; set; }

        public bool IsApproveNotification { get; set; }

        public bool IsApproveEvent { get; set; }
        [StringLength(200)]
        public string TeacherCode { get; set; }
    }

}

