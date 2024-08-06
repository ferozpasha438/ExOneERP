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

namespace CIN.Application.SchoolMgtDtos
{
    [AutoMap(typeof(TblSchoolMessages))]
    public class TblSchoolMessagesDto
    {
        public int Id { get; set; }
        public int Mobile { get; set; }
        public string SentBy { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool ReadFlag { get; set; }
        public DateTime ReadDateTime { get; set; }
    }

    [AutoMap(typeof(TblSysSchooScheduleEvents))]
    public class TblSysSchooScheduleEventsDto
    {
        public int Id { get; set; }
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
        public string BranchCode { get; set; }
        public bool IsActive { get; set; }
        public string NotesOnEvent { get; set; }
    }
    public class ApprovalEventsDto
    {
        public int Id { get; set; }
    }

    [AutoMap(typeof(TblSysSchoolNews))]
    public class TblSysSchoolNewsDto
    {
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

    [AutoMap(typeof(TblSysSchoolNewsMediaLib))]
    public class TblSysSchoolNewsMediaLibDto
    {
        public int Id { get; set; }
        public int NewId { get; set; }
        public string? Mediapath { get; set; }
        public string? MediaType { get; set; }
        public string? MediaNotes { get; set; }
        public bool IsActive { get; set; }
    }


    public class TblSchoolMeadiaNewsDto
    {
        public List<TblSysSchoolNewsDto> SchoolNewsList { get;set;}
        public List<TblSysSchoolNewsMediaLibDto> SchoolNewsDetailList { get; set; }
    }


    public class TblSchoolMeadiaDetailDto
    {
        public TblSysSchoolNewsDto SchoolNews { get; set; }
        public List<TblSysSchoolNewsMediaLibDto> SchoolNewsDetailList { get; set; }
    }

    

}
