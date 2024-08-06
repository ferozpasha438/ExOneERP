using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysLeaveTemplateMapping")]
    public class TblHRMSysLeaveTemplateMapping : PrimaryKey<int>
    {
        [ForeignKey(nameof(TemplateCode))]
        public TblHRMSysLeaveTemplate SysLeaveTemplate { get; set; }
        [Required]
        [StringLength(20)]
        public string TemplateCode { get; set; }

        [ForeignKey(nameof(LeaveTypeCode))]
        public TblHRMSysLeaveType SysLeaveType { get; set; }
        [Required]
        [StringLength(20)]
        public string LeaveTypeCode { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Count { get; set; } = 0;
    }
}
