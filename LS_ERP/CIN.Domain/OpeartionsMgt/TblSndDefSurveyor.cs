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
    [Table("tblSndDefSurveyor")]
    public class TblSndDefSurveyor : AutoActiveGenerateIdAuditableKey<int>
    {
        public int UserId { get; set; }
        [Key]
        [StringLength(20)]
        public string SurveyorCode { get; set; }
        [StringLength(200)]
        [Required]
        public string SurveyorNameEng { get; set; }
        [StringLength(200)]
        public string SurveyorNameArb { get; set; }
        [Required]
        [ForeignKey(nameof(Branch))]
        public TblErpSysCompanyBranch SysBranch { get; set; }
        public string Branch { get; set; }

    }
}
