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
    [Table("tblOpReasonCode")]
    public class TblOpReasonCode : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string ReasonCode { get; set; }
       
        [Required]
        [StringLength(50)]
        public string ReasonCodeNameEng { get; set; }
        [Required]
        [StringLength(50)]
        public string ReasonCodeNameArb { get; set; }
        [StringLength(500)]
        public string DescriptionEng { get; set; }
        [StringLength(500)]
        public string DescriptionArb { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsForCustomerVisit { get; set; } = false;
        public bool? IsForCustomerComplaint { get; set; } = false;
    }
}
