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
    [Table("HRM_TRAN_EmployeePrimarySites_Log")]
    public class HRM_TRAN_EmployeePrimarySites_Log // : AuditableEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EmployeePrimarySitesLogID { get; set; }

        public string EmployeeNumber { get; set; }
        public string SiteCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TransferredDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }

        public bool? IsReliever { get; set; }
    }
}
