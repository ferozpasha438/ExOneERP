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
        public string EventCreatedBy { get; set; }
        public DateTime EventCreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public string BranchCode { get; set; }
        public bool IsActive { get; set; }
    }
}
