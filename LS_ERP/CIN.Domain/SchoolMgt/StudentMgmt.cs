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
    [Table("tblDefSchoolStudentMaster")]
    public class TblDefSchoolStudentMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string StuRegNum { get; set; }
        public string StuAdmNum { get; set; }
        public DateTime? StuRegDate { get; set; }
        public DateTime? StuAdmDate { get; set; }
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
        public string PAddress1 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string BuildingName { get; set; }
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
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsPromoted { get; set; }
    }

    [Table("tblDefStudentFeeHeader")]
    public class TblDefStudentFeeHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        public bool IsPartiallyPaid { get; set; }   

    }


    [Table("tblDefStudentFeeDetails")]
    public class TblDefStudentFeeDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        public string TaxCode { get; set; }
    }

    [Table("tblDefStudentAttendance")]
    public class TblDefStudentAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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


    [Table("tblDefStudentApplyLeave")]
    public class TblDefStudentApplyLeave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    [Table("tblDefStudentAddress")]
    public class TblDefStudentAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string PAddress1 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone1 { get; set; }
        public string Mobile1 { get; set; }
        public string BuildingName { get; set; }
    }


    [Table("tblDefStudentGuardiansSiblings")]
    public class TblDefStudentGuardiansSiblings
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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


    [Table("tblDefStudentPrevEducation")]
    public class TblDefStudentPrevEducation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string NameOfInstitute { get; set; }
        public string ClassStudied { get; set; }
        public string LanguageMedium { get; set; }
        public string BoardName { get; set; }
        public string PassPercentage { get; set; }
        public int YearofPassing { get; set; }
    }

    [Table("tblDefStudentNotices")]
    public class TblDefStudentNotices
    {  
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public DateTime? ActionDate { get; set; }
    public string Description { get; set; }
    public string Description_Ar { get; set; }
   
    public string ClosedBy { get; set; }
    public DateTime? ClosedOn { get; set; }
}

    [Table("tblDefStudentNoticesReasonCode")]
    public class TblDefStudentNoticesReasonCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonType { get; set; }
        public string ReasonName1 { get; set; }
        public string ReasonName2 { get; set; }
        public bool RequireAction { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("tblDefStudentAcademics")]
    public class TblDefStudentAcademics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StuAdmNum { get; set; }
        public string Grade { get; set; }
        public string ExamDate { get; set; }
        public string ExamName { get; set; }
        public string Result { get; set; }
        public string PassPercent { get; set; }
        public int AcademicYear { get; set; }
    }





}
