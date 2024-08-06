using AutoMapper;
using CIN.Domain.SchoolMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CIN.Application.TeacherMgtDtos
{
    [AutoMap(typeof(TblSysSchoolPushNotificationParent))]
    public class TblSysSchoolPushNotificationParentDto
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
    }

    public class SchoolPushNotificationTeacherDto
    {
        // public int countNotification { get; set; }
        public List<TblSysSchoolPushNotificationParentDto> TeacherPushNotifications { get; set; }
    }

    public class TeacherNotificationCountDto
    {
        public int TotalCountNotification { get; set; }
        public int ReadCountNotification { get; set; }
        public int UnreadCountNotification { get; set; }
    }

    public class UpdateNotificationSuccessDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }

    }

    public class UpdateNotificationFailedDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }

    }


    //public class SendNotificationDto
    //{
    //    public int Id { get; set; }
    //    public string Contact { get; set; }
    //    public string StudentAdmNum { get; set; }
    //    public string Branch { get; set; }
    //    public string Grade { get; set; }
    //    public string Title { get; set; }
    //    public string Title_Ar { get; set; }
    //    public string NotifyMessage { get; set; }
    //    public string NotifyMessage_Ar { get; set; }

    //}
}
