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
    [Table("tblSndDefServiceMaster")]
    public class TblSndDefServiceMaster : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string ServiceCode { get; set; }
        [StringLength(200)]
        [Required]
        public string ServiceNameEng { get; set; }
        [StringLength(200)]
        public string ServiceNameArb { get; set; }
        [ForeignKey(nameof(SurveyFormCode))]
        public TblSndDefSurveyFormHead SysSurveyFormCode { get; set; }
        public string SurveyFormCode { get; set; }
    
    }
}
