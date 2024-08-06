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
    [Table("tblSndDefSurveyFormElementsMapp")]
    public class TblSndDefSurveyFormElementsMapp : AutoGenerateIdKey<int>
    {
        [ForeignKey(nameof(SurveyFormCode))]
        public TblSndDefSurveyFormHead SysSurveyFormCode { get; set; }
        [StringLength(20)]
        public string SurveyFormCode { get; set; }
        [ForeignKey(nameof(FormElementCode))]
        public TblSndDefSurveyFormElement SysFormElementCode { get; set; }
        [Required]
        [StringLength(20)]
        public string FormElementCode { get; set; }
    }
}
