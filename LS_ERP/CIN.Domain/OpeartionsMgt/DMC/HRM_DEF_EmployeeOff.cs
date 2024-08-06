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
    [Table("HRM_DEF_EmployeeOff")]
    public class HRM_DEF_EmployeeOff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        [StringLength(15)]
        public string Dow { get; set; }
        public string SiteCode { get; set; }
    }
}
