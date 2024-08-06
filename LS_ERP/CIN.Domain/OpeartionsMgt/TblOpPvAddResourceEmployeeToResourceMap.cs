using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpPvAddResourceEmployeeToResourceMap")]
    public class TblOpPvAddResourceEmployeeToResourceMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MapId { get; set; }

        public long PvAddResReqId { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string SkillSet { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        [StringLength(20)]
        public string DefShift { get; set; }
        public short OffDay { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }
        [StringLength(500)]
        public string FileUrl { get; set; }
        public int? FileUploadBy { get; set; }
    }
}
