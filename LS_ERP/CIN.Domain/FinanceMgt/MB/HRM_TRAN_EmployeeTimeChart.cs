//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIN.Domain.FinanceMgt.MB
//{
//    [Table("HRM_TRAN_EmployeeTimeChart")]
//    public class HRMTRANEmployeeTimeChart
//    {
//        public long Id { get; set; }
//        public long? EmployeeId { get; set; }
//        public string Name { get; set; }
//        public DateTime? Date { get; set; }
//        public TimeSpan? InTime { get; set; }
//        public TimeSpan? OutTime { get; set; }
//        public TimeSpan? Twt { get; set; }
//        public TimeSpan? Nwt { get; set; }
//        public DateTime? ShiftTime { get; set; }
//        public string AttnFlag { get; set; }
//        public bool? ApproveAttn1 { get; set; }
//        public bool? ApproveAttn2 { get; set; }
//        public bool? ApproveAttn3 { get; set; }
//        public TimeSpan? EstimatedInTime { get; set; }
//        public TimeSpan? EstimatedOutTime { get; set; }
//        public TimeSpan? EstimatedNetWorkingTime { get; set; }
//        public long? LateHours { get; set; }
//        public long? OverTimeHours { get; set; }
//        public long? NetWorkingTime { get; set; }
//        public long? ShiftId { get; set; }
//        public bool? IsLate { get; set; }
//        public bool? IsSpecialDay { get; set; }
//        public bool? IsPunchedOutNextDay { get; set; }
//        public string SiteCode { get; set; }
//        public byte? ShiftNumber { get; set; }
//        public long? ProjectId { get; set; }
//        public string ProjectCode { get; set; }
//        public long? SiteId { get; set; }
//        public string EmployeeNumber { get; set; }
//        public string ShiftCode { get; set; }
//        public string CustomerCode { get; set; }
//        public bool? IsApproved { get; set; }
//    }
//}
