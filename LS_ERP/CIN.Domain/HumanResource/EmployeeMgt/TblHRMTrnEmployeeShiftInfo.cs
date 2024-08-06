using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeShiftInfo")]
    public class TblHRMTrnEmployeeShiftInfo : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Monday Shift Code
        [ForeignKey(nameof(MondayShiftCode))]
        public TblHRMSysShift MondayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string MondayShiftCode { get; set; }

        //Tuesday Shift Code
        [ForeignKey(nameof(TuesdayShiftCode))]
        public TblHRMSysShift TuesdayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string TuesdayShiftCode { get; set; }

        //Wednesday Shift Code
        [ForeignKey(nameof(WednesdayShiftCode))]
        public TblHRMSysShift WednesdayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string WednesdayShiftCode { get; set; }

        //Thursday Shift Code
        [ForeignKey(nameof(ThursdayShiftCode))]
        public TblHRMSysShift ThursdayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string ThursdayShiftCode { get; set; }

        //Friday Shift Code
        [ForeignKey(nameof(FridayShiftCode))]
        public TblHRMSysShift FridayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string FridayShiftCode { get; set; }

        //Saturday Shift Code
        [ForeignKey(nameof(SaturdayShiftCode))]
        public TblHRMSysShift SaturdayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string SaturdayShiftCode { get; set; }

        //Sunday Shift Code
        [ForeignKey(nameof(SundayShiftCode))]
        public TblHRMSysShift SundayShift { get; set; }
        [Required]
        [StringLength(20)]
        public string SundayShiftCode { get; set; }
    }
}
