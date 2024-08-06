using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("HRM_TRAN_EmployeeTimeChart")]
    public class HRM_TRAN_EmployeeTimeChart // : AuditableEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long? EmployeeID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public TimeSpan? InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public TimeSpan? TWT { get; set; }
        public TimeSpan? NWT { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ShiftTime { get; set; }
        public char AttnFlag { get; set; }
        public bool? ApproveAttn1 { get; set; }
        public bool? ApproveAttn2 { get; set; }
        public bool? ApproveAttn3 { get; set; }
        public TimeSpan? EstimatedInTime { get; set; }
        public TimeSpan? EstimatedOutTime { get; set; }
        public TimeSpan? EstimatedNetWorkingTime { get; set; }
        public long? LateHours { get; set; }
        public long? OverTimeHours { get; set; }
        public long? NetWorkingTime { get; set; }
        public long? ShiftId { get; set; }
        public bool? IsLate { get; set; }
        public bool? IsSpecialDay { get; set; }
        public bool? IsPunchedOutNextDay { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        public byte? ShiftNumber { get; set; }
        public long? ProjectID { get; set; }
        [StringLength(50)]
        public string ProjectCode { get; set; }
        [StringLength(50)]
        public string CustomerCode { get; set; }
        public long? SiteID { get; set; }
        [StringLength(50)]
        public string EmployeeNumber { get; set; }
        [StringLength(20)]
        public string ShiftCode { get; set; }
        public bool? IsPrimarySite { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte? OPShiftNumber { get; set; }
        public bool? IsApproved { get; set; } = false;
    }
}
