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
   [Table("tblSchoolMessages")]
   public class TblSchoolMessages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Mobile { get; set; }
        public string SentBy { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool ReadFlag { get; set; }
        public DateTime ReadDateTime { get; set; }
    }



    [Table("tblSysSchooScheduleEvents")]
    public class TblSysSchooScheduleEvents
    {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Key]
    public DateTime HDate { get; set; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
    public string EventName { get; set; }
    public string EventNameAr { get; set; }
    public string EventDescription { get; set; }
    public string EventDescriptionAr { get; set; }
    public string EventCreatedBy { get; set; }
    public DateTime EventCreatedOn { get; set; }
    public bool IsApproved { get; set; }
    public string ApprovedBy { get; set; }
    [StringLength(200)]
    public string BranchCode { get; set; }
    public bool IsActive { get; set; }
    public string NotesOnEvent { get; set; }
    }


    [Table("tblSysSchoolNews")]
    public class TblSysSchoolNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewId { get; set; }
        public string Topic { get; set; }
        public string Topic_Ar { get; set; }
        public string Headlines { get; set; }
        public string Headlines_Ar { get; set; }
        public string NewsDetails { get; set; }
        public DateTime NewsDate { get; set; }
        public string? NewTumbnailImagePath { get; set; }
        public string PostedBy { get; set; }
        public bool? IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApproveDate { get; set; }
        
    }

    [Table("tblSysSchoolNewsMediaLib")]
    public class TblSysSchoolNewsMediaLib
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(NewId))]
        public TblSysSchoolNews SysSchoolNews { get; set; }
        public int NewId { get; set; }
        public string Mediapath { get; set; }
        public string MediaType { get; set; }
        public string MediaNotes { get; set; }
        public bool IsActive { get; set; }
    }


    [Table("tblSysSchoolPushNotificationParent")]
    public class TblSysSchoolPushNotificationParent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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
        public string UserId { get; set; }
    }

    [Table("tblSysSchoolNotifications")]
    public class TblSysSchoolNotifications
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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
        public string   MobileNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }

    }

    [Table("tblSysSchoolNotificationFilters")]
    public class TblSysSchoolNotificationFilters
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int NotificationId { get; set; }
        [StringLength(200)]
        public string BranchCode { get; set; }

        [StringLength(200)]
        public string GradeCode { get; set; }
        [StringLength(200)]
        public string NationalityCode { get; set; }
        [StringLength(200)]
        public string SectionCode { get; set; }
        [StringLength(200)]
        public string PTGroupCode { get; set; }
        [StringLength(200)]
        public string GenderCode { get; set; }
        [StringLength(200)]
        public string PickUpAndDropZone { get; set; }
    }

  
    [Table("tblSysNotificaticationTemplate")]
    public class TblSysNotificaticationTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Template_En { get; set; }
        public string Template_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
