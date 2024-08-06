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
    public class AllStudentMasterDataDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public DateTime? StuAdmDate { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Alias { get; set; }
        public string GenderCode { get; set; }
        public int Age { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string PTGroupCode { get; set; }
        public string GradeSectionCode { get; set; }
        public string LangCode { get; set; }
        public string NatCode { get; set; }
        public string ReligionCode { get; set; }
        public string StuIDNumber { get; set; }
        public string IDNumber { get; set; }
        public string MotherToungue { get; set; }
        public string RegisteredPhone { get; set; }
        public string RegisteredEmail { get; set; }
        public bool IsActive { get; set; }
        public string StudentImageFileName { get; set; }
        public IFormFile StudentImage { get; set; }
        public string FeeStructCode { get; set; }
        public decimal? TotFeeAmount { get; set; }
        public decimal? PaidFees { get; set; }
        public decimal? NetFeeAmount { get; set; }
        public bool TransportationRequired { get; set; }
        public bool TaxApplicable { get; set; }
        public string PickNDropZone { get; set; }
        public decimal? TransportationFee { get; set; }
        public string VehicleTransport { get; set; }
        public string BuildingName { get; set; }
        public string PAddress1 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string Mobile { get; set; }
        public string FatherName { get; set; }
        public string FatherMobile { get; set; }
        public string FatherEmail { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherDesignation { get; set; }
        public string FatherSignatureFileName { get; set; }
        public IFormFile FatherSignature { get; set; }
        public string MotherName { get; set; }
        public string MotherMobile { get; set; }
        public string MotherEmail { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherDesignation { get; set; }
        public string MotherSignatureFileName { get; set; }
        public IFormFile MotherSignature { get; set; }
        public string BloodGroup { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public bool SpecialAssistance { get; set; }
        public string SpecialAssistanceNotes { get; set; }
        public bool PhysicalDisability { get; set; }
        public string PhysicalDisabilityNotes { get; set; }
        public int AcademicsScale { get; set; }
        public int AttentivenessScale { get; set; }
        public int HomeWorkScale { get; set; }
        public int ProjectWorkScale { get; set; }
        public int SportsPhysicalScale { get; set; }
        public int DiciplineAttitude { get; set; }
        //    public string BranchCode { get; set; }
    }


    public class SchoolStudentMasterGradeDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public DateTime? StuAdmDate { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Alias { get; set; }
        public string GenderCode { get; set; }
        public int Age { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string PTGroupCode { get; set; }
        public string GradeSectionCode { get; set; }
        public string LangCode { get; set; }
        public string NatCode { get; set; }
        public string ReligionCode { get; set; }
        public string StuIDNumber { get; set; }
        public string IDNumber { get; set; }
        public string MotherToungue { get; set; }
        public string RegisteredPhone { get; set; }
        public string RegisteredEmail { get; set; }
        public bool IsActive { get; set; }
        public string Image1Path { get; set; }
        public IFormFile StudentImage { get; set; }
        public string FeeStructCode { get; set; }
        public decimal? TotFeeAmount { get; set; }
        public decimal? PaidFees { get; set; }
        public decimal? NetFeeAmount { get; set; }
        public bool TransportationRequired { get; set; }
        public bool TaxApplicable { get; set; }
        public string PickNDropZone { get; set; }
        public decimal? TransportationFee { get; set; }
        public string VehicleTransport { get; set; }
        public string BuildingName { get; set; }
        public string PAddress1 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Mobile { get; set; }
        public string FatherName { get; set; }
        public string FatherMobile { get; set; }
        public string FatherEmail { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherDesignation { get; set; }
        public string FatherSignatureFileName { get; set; }
        public IFormFile FatherSignature { get; set; }
        public string MotherName { get; set; }
        public string MotherMobile { get; set; }
        public string MotherEmail { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherDesignation { get; set; }
        public string MotherSignatureFileName { get; set; }
        public IFormFile MotherSignature { get; set; }
        public string BloodGroup { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public bool SpecialAssistance { get; set; }
        public string SpecialAssistanceNotes { get; set; }
        public bool PhysicalDisability { get; set; }
        public string PhysicalDisabilityNotes { get; set; }
        public int AcademicsScale { get; set; }
        public int AttentivenessScale { get; set; }
        public int HomeWorkScale { get; set; }
        public int ProjectWorkScale { get; set; }
        public int SportsPhysicalScale { get; set; }
        public int DiciplineAttitude { get; set; }
        public string FileName { get; set; }
    }


    [AutoMap(typeof(TblDefSchoolStudentMaster))]
    public class TblDefSchoolStudentMasterDto
    {
        public int Id { get; set; }
        public string StuRegNum { get; set; }
        public string StuAdmNum { get; set; }
        public DateTime? StuAdmDate { get; set; }
        public DateTime? StuRegDate { get; set; }
        public string StuName { get; set; }
        public string StuName2 { get; set; }
        public string Alias { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DateofBirth { get; set; }
        public int Age { get; set; }
        public string GradeCode { get; set; }
        public string GradeSectionCode { get; set; }
        public string LangCode { get; set; }
        public int AcademicYear { get; set; }
        public string GenderCode { get; set; }
        public string PTGroupCode { get; set; }
        public string StuIDNumber { get; set; }
        public string IDNumber { get; set; }
        public string NatCode { get; set; }
        public string ReligionCode { get; set; }
        public string MotherToungue { get; set; }
        public string FeeStructCode { get; set; }
        public bool TransportationRequired { get; set; }
        public string PickNDropZone { get; set; }
        public decimal TransportationFee { get; set; }
        public string VehicleTransport { get; set; }
        public string RegisteredPhone { get; set; }
        public string RegisteredEmail { get; set; }
        public bool IsActive { get; set; }
        public string PAddress1 { get; set; }
        public string BuildingName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }
        public string Image1Path { get; set; }
        public string Image2Path { get; set; }
        public string AdmissionType { get; set; }
        public string ShortListDate { get; set; }
        public string ShortListedBy { get; set; }
        public string DateofAdmission { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string StuConvDate { get; set; }
        public string StuConvBy { get; set; }
        public string BloodGroup { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public bool PhysicalDisability { get; set; }
        public string PhysicalDisabilityNotes { get; set; }
        public bool WearSpects { get; set; }
        public bool SpecialAssistance { get; set; }
        public string SpecialAssistanceNotes { get; set; }
        public int AcademicsScale { get; set; }
        public int AttentivenessScale { get; set; }
        public int HomeWorkScale { get; set; }
        public int ProjectWorkScale { get; set; }
        public int SportsPhysicalScale { get; set; }
        public int DiciplineAttitude { get; set; }
        public string SignatureImage1 { get; set; }
        public string SignatureImage2 { get; set; }
        public string BranchCode { get; set; }
        public bool TaxApplicable { get; set; }
        
    }


    [AutoMap(typeof(TblDefStudentFeeHeader))]
    public class TblDefStudentFeeHeaderDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public DateTime FeeDueDate { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidOn { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string JvNumber { get; set; }
        public int AcademicYear { get; set; }
        public bool IsCompletelyPaid { get; set; }
        public decimal TaxAmount { get; set; }
    }


    [AutoMap(typeof(TblDefStudentFeeDetails))]
    public class TblDefStudentFeeDetailsDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public string FeeCode { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal MaxDiscPer { get; set; }
        public decimal DiscPer { get; set; }
        public decimal NetDiscAmt { get; set; }
        public decimal NetFeeAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsLateFee { get; set; }
        public bool IsAddedManaully { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsVoided { get; set; }
        public string VoidedBy { get; set; }
        public string VoidReason { get; set; }
        public int AcademicYear { get; set; }
        public decimal TaxAmount { get; set; }
        public string TaxCode { get; set; }
    }


    [AutoMap(typeof(TblDefStudentAttendance))]
    public class TblDefStudentAttendanceDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public DateTime AtnDate { get; set; }
        public DateTime AtnTimeIn { get; set; }
        public DateTime? AtnTimeOut { get; set; }
        public string AtnFlag { get; set; }
        public string IsLeave { get; set; }
        public string LeaveCode { get; set; }
        public int AcademicYear { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolHolidayCalanderStudent))]
    public class TblSysSchoolHolidayCalanderStudentDto
    {

        public int Id { get; set; }
        public DateTime HDate { get; set; }
        public string HName { get; set; }
        public string HName2 { get; set; }
        public string BranchCode { get; set; }
    }

    [AutoMap(typeof(TblDefStudentApplyLeave))]
    public class TblDefStudentApplyLeaveDto
    {
        public int Id { get; set; }
        public string RegisteredPhone { get; set; }
        public string RegisteredEmail { get; set; }
        public string StuAdmNum { get; set; }
        public string LeaveCode { get; set; }
        public string LeaveReason { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string LeaveMessage { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public string Attachment3 { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovalRemarks { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int AcademicYear { get; set; }
    }
    public class StudentLeaveCodesDto
    {
        public string LeaveCode { get; set; }
        public string LeaveName { get; set; }
        public string LeaveName2 { get; set; }

    }
    public class TblSchoolStudentAttnListDto
    {
        public string WeekOff1 { get; set; }
        public string WeekOff2 { get; set; }
        public List<TblDefStudentAttendanceDto> StuAttnList { get; set; }
        public List<TblSysSchoolHolidayCalanderStudentDto> StuHolidayList { get; set; }
        public List<TblDefStudentApplyLeaveDto> StuLeaveList { get; set; }
    }

    [AutoMap(typeof(TblDefStudentAddress))]
    public class TblDefStudentAddressDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string PAddress1 { get; set; }
        public string BuildingName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone1 { get; set; }
        public string Mobile1 { get; set; }
    }

    [AutoMap(typeof(TblDefStudentGuardiansSiblings))]
    public class TblDefStudentGuardiansSiblingsDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string RelationType { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string Occupation { get; set; }
        public string Deisgnation { get; set; }
        public string Mobile1 { get; set; }
        public string email { get; set; }
        public string Remarks { get; set; }
    }

    [AutoMap(typeof(TblDefStudentPrevEducation))]
    public class TblDefStudentPrevEducationDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string NameOfInstitute { get; set; }
        public string ClassStudied { get; set; }
        public string LanguageMedium { get; set; }
        public string BoardName { get; set; }
        public string PassPercentage { get; set; }
        public int YearofPassing { get; set; }
    }

    [AutoMap(typeof(TblDefStudentNotices))]
    public class TblDefStudentNoticesDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string PosNeg { get; set; }
        public DateTime NoticeDate { get; set; }
        public string ReasonCode { get; set; }
        public string ReportedBy { get; set; }
        public string Remarks { get; set; }
        public string ActionItems { get; set; }
        public string ActionRemarks { get; set; }
        public bool IsApproved { get; set; }
        public string AprovedBy { get; set; }
        public string IsClosed { get; set; }
        public string ActionTaken { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public DateTime? ActionDate { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
    }

    [AutoMap(typeof(TblDefStudentNoticesReasonCode))]
    public class TblDefStudentNoticesReasonCodeDto
    {
        public int Id { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonType { get; set; }
        public string ReasonName1 { get; set; }
        public string ReasonName2 { get; set; }
        public bool RequireAction { get; set; }
        public bool IsActive { get; set; }
    }

    [AutoMap(typeof(TblDefStudentAcademics))]
    public class TblDefStudentAcademicsDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string Grade { get; set; }
        public string ExamDate { get; set; }
        public string ExamName { get; set; }
        public string Result { get; set; }
        public string PassPercent { get; set; }
        public int AcademicYear { get; set; }
    }
    public class StudentNoticesListDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string PosNeg { get; set; }
        public DateTime NoticeDate { get; set; }
        public string ReasonType { get; set; }
        public string ReasonCode { get; set; }
        public string ReportedBy { get; set; }
        public string Remarks { get; set; }
        public string ActionItems { get; set; }
        public string ActionRemarks { get; set; }
        public bool IsApproved { get; set; }
        public string AprovedBy { get; set; }
        public string IsClosed { get; set; }
        public string ActionTaken { get; set; }
        public DateTime? ActionDate { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
    }
    public class StudentFeeDetailsDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public string FeeCode { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal MaxDiscPer { get; set; }
        public decimal DiscPer { get; set; }
        public decimal NetDiscAmt { get; set; }
        public decimal NetFeeAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsLateFee { get; set; }
        public bool IsAddedManaully { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsVoided { get; set; }
        public string VoidedBy { get; set; }
        public string VoidReason { get; set; }
        public int AcademicYear { get; set; }
        public string FeeName { get; set; }
        public string FeeName2 { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class StudentDetailsForFeeTransactionDto
    {
        public string StuAdmNum { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string StudentName { get; set; }
        public string StudentNameAR { get; set; }
        public string BranchCode { get; set; }
        public decimal FeeAmount { get; set; }
    }

    public class BranchEventsHolidaysDataDto
    {
        public BranchEventsHolidaysDataDto()
        {
            EventsHolidaysDataList = new();
        }
        public string BranchCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<EventsHolidaysData> EventsHolidaysDataList { get; set; }
    }
    public class EventsHolidaysData
    {
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public string EventNameAr { get; set; }
        public int EventType { get; set; }

    }
    public class CalendarRequestDto
    {
        public string BranchCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class StudentAttandanceDatewiseDto
    {
        public DateTime AttDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Flag { get; set; }
        public string LeaveCode { get; set; }
    }
    public class StudentAttandanceDataDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NoOfDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LeaveDays { get; set; }
        public int HolidayDays { get; set; }
    }
    public class StudentAttandanceResultDto
    {
        public StudentAttandanceResultDto()
        {
            AttnDaywiseDataList = new();
            StudentAttandanceData = new();
        }
        public StudentAttandanceDataDto StudentAttandanceData { get; set; }
        public List<StudentAttandanceDatewiseDto> AttnDaywiseDataList { get; set; }
    }

    public class StudentLeaveDataDto
    {
        public DateTime LeaveDate { get; set; }
        public string LeaveCode { get; set; }
    }

    public class StudentFeeHeaderDetailsDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public DateTime FeeDueDate { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidOn { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string JvNumber { get; set; }
        public int AcademicYear { get; set; }
        public bool IsCompletelyPaid { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class StudentFeePayDetailsDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public DateTime FeeDueDate { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidOn { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string JvNumber { get; set; }
        public int AcademicYear { get; set; }
        public bool IsCompletelyPaid { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LateFee { get; set; }
    }

    public class ChartAttandanceDto
    {
        public int TypeID { get; set; }
        public int TotalStudents { get; set; }
        public int PresentStudents { get; set; }
        public int LeaveStudents { get; set; }
        public int AbsentStudents { get; set; }
    }
    public class DashboardEventDto
    {
        public string EventName { get; set; }
        public string EventName2 { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class DashboardStudentDto
    {
        public string StudentName { get; set; }
        public string StudentName2 { get; set; }
        public string StuAdmNum { get; set; }
        public string GradeCode { get; set; }
        public string Grade { get; set; }
        public string Grade2 { get; set; }
    }
    public class SchoolDashboardDto
    {
        public SchoolDashboardDto()
        {
            ChartAttandanceData = new();
            DashboardEvents = new();
            DashboardStudents = new();
        }
        public string BranchCode { get; set; }
        public int TotalStudents { get; set; }
        public int StudentsOnLeave { get; set; }
        public int FeeDueStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int NewRegistrations { get; set; }
        public decimal FeeDueTotal { get; set; }
        public List<ChartAttandanceDto> ChartAttandanceData { get; set; }
        public List<DashboardEventDto> DashboardEvents { get; set; }
        public List<DashboardStudentDto> DashboardStudents { get; set; }
    }

    public class StudentFeeHeaderPaymentDto
    {
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public DateTime FeeDueDate { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidOn { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string JvNumber { get; set; }
        public int AcademicYear { get; set; }
        public bool IsCompletelyPaid { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsPartiallyPaid { get; set; }
    }
}
