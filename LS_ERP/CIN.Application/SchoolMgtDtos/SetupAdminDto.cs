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
using CIN.Application.Common;
using Microsoft.AspNetCore.Http;

namespace CIN.Application.SchoolMgtDto
{
    [AutoMap(typeof(TblSysSchoolAcademicBatches))]
    public class TblSysSchoolAcademicBatchesDto
    {

        public int Id { get; set; } //Identity

        public int AcademicYear { get; set; }  //Primary Key
        [Required]
        public DateTime AcademicStartDate { get; set; }
        [Required]
        public DateTime AcademicEndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }


    [AutoMap(typeof(TblSysSchoolAcademicsSubects))]
    public class TblSysSchoolAcademicsSubectsDto
    {

        public int Id { get; set; }       //Identity

        public string SubCodes { get; set; }  //Primary Key
        [Required]
        public string SubName { get; set; }
        public string SubName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }


    [AutoMap(typeof(TblSysSchoolSectionsSection))]
    public class TblSysSchoolSectionsSectionDto
    {

        public int Id { get; set; }

        public string SectionCode { get; set; }
        [Required]
        public string SectionName { get; set; }
        public string SectionName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class TblSysSchoolSectionsSectionDataDto
    {

        public int Id { get; set; }

        public string SectionCode { get; set; }
        [Required]
        public string SectionName { get; set; }
        public string SectionName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public IFormFile UploadFile { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolAcedemicClassGrade))]
    public class TblSysSchoolAcedemicClassGradeDto
    {

        public int Id { get; set; }

        public string GradeCode { get; set; }
        [Required]
        public string GradeName { get; set; }
        public string GradeName2 { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int? GradeOrder { get; set; }

    }

    [AutoMap(typeof(TblSysSchoolSemister))]
    public class TblSysSchoolSemisterDto
    {

        public int Id { get; set; }

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


    [AutoMap(typeof(TblSysSchoolPETCategory))]
    public class TblSysSchoolPETCategoryDto
    {

        public int Id { get; set; }

        public string PETCode { get; set; }
        [Required]
        public string PETName { get; set; }
        public string PETName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolNationality))]
    public class TblSysSchoolNationalityDto
    {

        public int Id { get; set; }

        public string NatCode { get; set; }
        [Required]
        public string NatName { get; set; }
        public string NatName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolStuLeaveType))]
    public class TblSysSchoolStuLeaveTypeDto
    {

        public int Id { get; set; }

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

    [AutoMap(typeof(TblSysSchoolPayTypes))]
    public class TblSysSchoolPayTypesDto
    {

        public int Id { get; set; }

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

    [AutoMap(typeof(TblSysSchoolReligion))]
    public class TblSysSchoolReligionDto
    {

        public int Id { get; set; }

        public string RegCode { get; set; }
        [Required]
        public string RegName { get; set; }
        public string RegName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }


    [AutoMap(typeof(TblSysSchoolLanguages))]
    public class TblSysSchoolLanguagesDto
    {

        public int Id { get; set; }

        public string LangCode { get; set; }
        [Required]
        public string LangName { get; set; }
        public string LangName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }



    [AutoMap(typeof(TblSysSchoolGender))]
    public class TblSysSchoolGenderDto
    {

        public int Id { get; set; }

        public string GenderCode { get; set; }
        [Required]
        public string GenderName { get; set; }
        public string GenderName2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }


    [AutoMap(typeof(TblSysSchoolFeeTerms))]
    public class TblSysSchoolFeeTermsDto
    {

        public int Id { get; set; }

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

    [AutoMap(typeof(TblSysSchoolFeeType))]
    public class TblSysSchoolFeeTypeDto
    {

        public int Id { get; set; }

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

    [AutoMap(typeof(TblSysSchoolFeeStructureHeader))]
    public class TblSysSchoolFeeStructureHeaderDto
    {
        public int Id { get; set; }
        public string FeeStructCode { get; set; }

        public string GradeCode { get; set; }

        public string BranchCode { get; set; }
        public string FeeStructName { get; set; }
        public string FeeStructName2 { get; set; }
        public bool ApplyLateFee { get; set; }

        public string LateFeeCode { get; set; }
        public decimal ActualFeePayable { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolFeeStructureDetails))]
    public class TblSysSchoolFeeStructureDetailsDto
    {

        public int Id { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public string FeeCode { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal MaxDiscPer { get; set; }
        public decimal ActualFeeAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public List<TblSysSchoolFeeStructureDetailsDto> FeeTermCodeDetails { get; set; }
    }



    public class TblSysSchoolFeeStructureDto : TblSysSchoolFeeStructureHeaderDto
    {
        public List<TblSysSchoolFeeStructureDetailsDto> FeeDetailList { get; set; }
    }

    //public class SchoolFeeDetailsforAllTermDto
    //{
    //    public string FeeStructCode { get; set; }
    //    public string TermCode { get; set; }
    //    public decimal FeeAmount { get; set; }

    //   public List<SchoolFeeDetailsforAllTermDto> FeeDetailList1 { get; set; }

    //}


    public class CustomSysSchoolFeeStructureDetailsDto
    {
        public string TermCode { get; set; }
        public int TotalCount { get; set; }
        public decimal FeeAmount { get; set; }
    }



    [AutoMap(typeof(TblSysSchoolGradeSectionMapping))]
    public class TblSysSchoolGradeSectionMapping1Dto
    {
        public int Id { get; set; }
        //[Required]
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

    public class GradeSectionMappingDto
    {
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public decimal MaxStrength { get; set; }
        public decimal MinStrength { get; set; }
        public decimal AvgStrength { get; set; }
        public string UploadFileName { get; set; }
        public IFormFile UploadFile { get; set; }
        public int Page { get; set; }
        public string SectionCodes { get; set; }
    }


    public class TblSysSchoolGradeSectionMappingDto
    {
        [Required]
        public string GradeCode { get; set; }
        public List<GradeSectionMappingDto> SchoolGradeSectionlist { get; set; }
    }


    [AutoMap(typeof(TblSysSchoolGradeSubjectMapping))]
    public class TblSysSchoolGradeSubjectMapping1Dto
    {
        public int Id { get; set; }

        [Required]
        public string GradeCode { get; set; }
        [Required]
        public string SemisterCode { get; set; }
        [Required]
        public string SubCodes { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
    public class SchoolGradeSubjectMappingDto
    {
        public string GradeCode { get; set; }
        public string SemisterCode { get; set; }
        public string SubCodes { get; set; }
    }
    public class TblSysSchoolGradeSubjectMappingDto
    {
        [Required]
        public string GradeCode { get; set; }
        [Required]
        public string SemisterCode { get; set; }
        [Required]
        public string[] SubCodes { get; set; }

        public List<TblSysSchoolGradeSubjectMapping1Dto> SchoolGradeSubjectlist { get; set; }
    }

    public class TblSysSchoolGradeSectionListDto
    {
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string SectionName2 { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolBranches))]
    public class TblSysSchoolBranchesDto
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        public string BranchNameAr { get; set; }
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
        public DateTime? StartAcademicDate { get; set; }
        public DateTime? EndAcademicDate { get; set; }
        public string WeekOff1 { get; set; }
        public string WeekOff2 { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }
        public string BranchPrefix { get; set; }
        public int NextStuNum { get; set; }
        public int NextFeeVoucherNum { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }

    [AutoMap(typeof(TblSysSchoolSchedule))]
    public class TblSysSchoolScheduleDto
    {
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

    public class SchoolBranchesDto
    {
        public SchoolBranchesDto()
        {
            SchoolBranchesAuthorityList = new();
        }
        public int Id { get; set; }
        [Required]
        public string BranchCode { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string BranchNameAr { get; set; }
        public DateTime? StartAcademicDate { get; set; }
        public DateTime? EndAcademicDate { get; set; }


        public string FinBranch { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
        [Required]
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

        public string GeoLat { get; set; }

        public string GeoLong { get; set; }
        public string BranchPrefix { get; set; }
        public int? NextStuNum { get; set; }
        public int? NextFeeVoucherNum { get; set; }
        public string BranchNotification_Moderator { get; set; }
        public string Default_InTime { get; set; }
        public string Default_OutTime { get; set; }
        public List<SchoolBranchesAuthorityDto> SchoolBranchesAuthorityList { get; set; }

    }

    public class SchoolBranchesAuthorityDto
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string BranchCode { get; set; }
        public int? Level { get; set; }

        public bool IsApproveLeave { get; set; }

        public bool IsApproveDisciPlinaryAction { get; set; }

        public bool IsApproveNotification { get; set; }

        public bool IsApproveEvent { get; set; }
        public string TeacherCode { get; set; }
    }
    [AutoMap(typeof(TblSysSchoolNotifications))]
    public class SysSchoolNotificationsDto
    {
        public int Id { get; set; }
        public int NotificationType { get; set; }
        [StringLength(200)]
        public int AcadamicYear { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle_Ar { get; set; }

        [Required]
        public string NotificationMessage { get; set; }
        [Required]
        public string NotificationMessage_Ar { get; set; }
        public string MobileNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }

    }

    public class TeacherNotificationsDto
    {
        public int Id { get; set; }
        public int NotificationType { get; set; }
    }
    public class IndividualNotificationsDto
    {
        public int Id { get; set; }
        public int NotificationType { get; set; }
        public string Code { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle_Ar { get; set; }

        [Required]
        public string NotificationMessage { get; set; }
        [Required]
        public string NotificationMessage_Ar { get; set; }

    }

    public class BulkNotificationsDto
    {
        public int Id { get; set; }
        public int NotificationType { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string NationalityCode { get; set; }
        public string PTGroupCode { get; set; }
        public string GenderCode { get; set; }
        public string PickUpAndDropZone { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle_Ar { get; set; }

        [Required]
        public string NotificationMessage { get; set; }
        [Required]
        public string NotificationMessage_Ar { get; set; }

    }

    public class EditBulkNotificationsDto
    {
        public int Id { get; set; }
        public int NotificationType { get; set; }
        public int AcadamicYear { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string NationalityCode { get; set; }
        public string PTGroupCode { get; set; }
        public string GenderCode { get; set; }
        public string PickUpAndDropZone { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle { get; set; }
        [Required]
        [StringLength(1000)]
        public string NotificationTitle_Ar { get; set; }

        [Required]
        public string NotificationMessage { get; set; }
        [Required]
        public string NotificationMessage_Ar { get; set; }

        public bool IsApproved { get; set; }

    }

    public class SchoolNotificationDto
    {

        public int Id { get; set; }
        public int MsgNoteId { get; set; }
        public DateTime? NotifyDate { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        public string NotifyMessage { get; set; }
        public string NotifyMessage_Ar { get; set; }
        public string RegisteredMobile { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDateTime { get; set; }
        public string NotifyTo { get; set; }
        public string FromUserId { get; set; }
        public string FromName { get; set; }
        public string UserID { get; set; }
    }

    public class SchoolTermDuePaymentDto
    {
        public string BranchCode { get; set; }
        public string StuAdmNum { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public string GradeCode { get; set; }
        public string GradeSectionCode { get; set; }
        //public Dictionary<string,decimal> TermDues { get; set; }

        public decimal Term1 { get; set; }
        public decimal Term2 { get; set; }
        public decimal Term3 { get; set; }
        public decimal Term4 { get; set; }
        public decimal FeeDue { get; set; }
    }

    public class AcademicFeeTransactionReportDto
    {
        public string BranchCode { get; set; }
        public string StuAdmNum { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public string GradeCode { get; set; }
        public string GradeSectionCode { get; set; }
        public string FeeStructCode { get; set; }
        public decimal TotalFee { get; set; }
        public decimal Tax { get; set; }
        public decimal NetFee { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }

    }

    public class StudentFeeListReportDto
    {
        public string BranchCode { get; set; }
        public string StuAdmNum { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public string GradeCode { get; set; }
        public string GradeSectionCode { get; set; }
        public string Nationality { get; set; }
        public string ParentContact { get; set; }
        public string FeeStuctureCode { get; set; }
        public decimal FeeAmount { get; set; }
        public bool Due { get; set; }
    }

    public class FeeStructureSummaryReportDto
    {
        public int Id { get; set; }
        public string StructCode { get; set; }
        public string FeeStructureName { get; set; }
        public string GradeCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal NetFee { get; set; }
    }

    public class FeeStructureDetailsReportDto
    {
        public string StructCode { get; set; }
        public string FeeStructureName { get; set; }
        public string FeeName { get; set; }
        public string GradeCode { get; set; }
        public string TermCode { get; set; }
        public string FeeCode { get; set; }
        public string FeeTypeName { get; set; }
        public string FeeTypeName2 { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal NetFee { get; set; }
    }

    public class GradeDoumentDto
    {
        public string GradeCode { get; set; }
        public string UploadFileName { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}
