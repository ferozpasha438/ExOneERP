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
    [Table("tblHRMTrnEmployeeLanguageInfo")]
    public class TblHRMTrnEmployeeLanguageInfo : PrimaryKey<int>
    {
        //EmployeeId
        [ForeignKey(nameof(EmployeeId))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        //Language Code
        [ForeignKey(nameof(LanguageCode))]
        public TblHRMSysLanguage SysLanguage { get; set; }
        [Required]
        [StringLength(20)]
        public string LanguageCode { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSpeak { get; set; }
    }
}
