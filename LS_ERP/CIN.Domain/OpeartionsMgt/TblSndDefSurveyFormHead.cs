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
    [Table("tblSndDefSurveyFormHead")]
    public class TblSndDefSurveyFormHead : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string SurveyFormCode { get; set; }
        [StringLength(200)]
        public string SurveyFormNameArb { get; set; }
        [Required]
        [StringLength(200)]
        public string SurveyFormNameEng { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }

    }
}
