using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpMonthlyRoasterForSite")]
    public class TblOpMonthlyRoasterForSite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        public short Month { get; set; }
        public short Year { get; set; }
        [StringLength(20)]
        public string SkillsetCode { get; set; }
        [StringLength(100)]
        public string SkillsetName { get; set; }
        [StringLength(20)]
        public string S1 { get; set; }
        [StringLength(20)]
        public string S2 { get; set; }
        [StringLength(20)]
        public string S3 { get; set; }
        [StringLength(20)]
        public string S4 { get; set; }
        [StringLength(20)]
        public string S5 { get; set; }
        [StringLength(20)]
        public string S6 { get; set; }
        [StringLength(20)]
        public string S7 { get; set; }
        [StringLength(20)]
        public string S8 { get; set; }
        [StringLength(20)]
        public string S9 { get; set; }
        [StringLength(20)]
        public string S10 { get; set; }
        [StringLength(20)]
        public string S11 { get; set; }
        [StringLength(20)]
        public string S12 { get; set; }
        [StringLength(20)]
        public string S13 { get; set; }
        [StringLength(20)]
        public string S14 { get; set; }
        [StringLength(20)]
        public string S15 { get; set; }
        [StringLength(20)]
        public string S16 { get; set; }
        [StringLength(20)]
        public string S17 { get; set; }
        [StringLength(20)]
        public string S18 { get; set; }
        [StringLength(20)]
        public string S19 { get; set; }
        [StringLength(20)]
        public string S20 { get; set; }
        [StringLength(20)]
        public string S21 { get; set; }
        [StringLength(20)]
        public string S22 { get; set; }
        [StringLength(20)]
        public string S23 { get; set; }
        [StringLength(20)]
        public string S24 { get; set; }
        [StringLength(20)]
        public string S25 { get; set; }
        public string S26 { get; set; }
        [StringLength(20)]
        public string S27 { get; set; }
        [StringLength(20)]
        public string S28 { get; set; }
        [StringLength(20)]
        public string S29 { get; set; }
        [StringLength(20)]
        public string S30 { get; set; }
        [StringLength(20)]
        public string S31 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MonthStartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MonthEndDate { get; set; }
        public long EmployeeID { get; set; }

        public long MapId { get; set; }
        public bool IsPrimaryResource { get; set; }
    }
}
