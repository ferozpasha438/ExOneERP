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
    [Table("tblOpProposalTemplate")]
    public class TblOpProposalTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
         [StringLength(20)]
        public string CustomerCode { get; set; }


        
        public string TitleOfService { get; set; }
        public string CoveringLetter { get; set; }
        public string Commercial { get; set; }
        public string Notes { get; set; }
        public string IssuingAuthority { get; set; }


        public string TitleOfServiceArb { get; set; }
        public string CoveringLetterArb { get; set; }
        public string CommercialArb { get; set; }
        public string NotesArb { get; set; }
        public string IssuingAuthorityArb { get; set; }
        public int CreatedBy { get; set; }
    }
}
