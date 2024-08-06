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
    [Table("tblOpEmployeeTransResign")]
    public class TblOpEmployeeTransResign : AuditableEntity<long>
    {
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AttnDate { get; set; }
        public bool TR { get; set; }
        public bool R { get; set; }
        public string Remarks { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        
    }
}
