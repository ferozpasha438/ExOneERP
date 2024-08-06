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
    [Table("tblSndDefUnitMaster")]
    public class TblSndDefUnitMaster : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string UnitCode	 { get; set; }
        [StringLength(200)]
        [Required]
        public string UnitNameEng { get; set; }
        [StringLength(200)]
        public string UnitNameArb { get; set; }

    }
}
