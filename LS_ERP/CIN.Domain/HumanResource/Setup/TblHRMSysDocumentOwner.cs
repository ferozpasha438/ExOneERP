using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysDocumentOwner")]
    public class TblHRMSysDocumentOwner : AuditableEntity<int>
    {
        [StringLength(20)]
        [Key]
        public string DocumentOwnerCode { get; set; }
        [Required]
        [StringLength(100)]
        public string DocumentOwnerName { get; set; }
        [Required]
        [StringLength(100)]
        public string DocumentOwnerNameAr { get; set; }
    }
}
