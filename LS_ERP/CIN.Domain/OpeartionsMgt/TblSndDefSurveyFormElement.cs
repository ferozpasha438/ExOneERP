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
    [Table("tblSndDefSurveyFormElement")]
    public class TblSndDefSurveyFormElement : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string FormElementCode { get; set; }
        [StringLength(200)]
        [Required]
        public string ElementEngName { get; set; }
        [StringLength(200)]
        public string ElementArbName { get; set; }
        [StringLength(20)]
        public string ElementType { get; set; }
        [StringLength(500)]
        public string ListValueString { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}
