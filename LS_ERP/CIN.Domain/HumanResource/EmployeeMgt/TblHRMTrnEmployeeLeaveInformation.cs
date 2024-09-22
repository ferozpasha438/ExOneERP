using CIN.Domain.HumanResource.Setup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeLeaveInformation")]
    public class TblHRMTrnEmployeeLeaveInformation : AuditableCreatedEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        [ForeignKey(nameof(TemplateCode))]
        public TblHRMSysLeaveTemplate SysLeaveTemplate { get; set; }
        //[Required]
        [StringLength(20)]
        public string TemplateCode { get; set; }

        [ForeignKey(nameof(LeaveTypeCode))]
        public TblHRMSysLeaveType SysLeaveType { get; set; }
        [Required]
        [StringLength(20)]
        public string LeaveTypeCode { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal Assigned { get; set; } = 0; //Total Default Leaves

        [Column(TypeName = "decimal(18, 3)")] //Total Taken Leaves
        public decimal Availed { get; set; } = 0;
        public DateTime TranDate { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }

    }
}
