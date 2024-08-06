using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{

    [Table("tblOpProposalCosting")]
    public class TblOpProposalCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int ProjectBudgetEstimationId { get; set; }
        [StringLength(20)]
        public string SkillSetCode { get; set; }        //only for resource costing
        public string ItemEng { get; set; }
        public string ItemArab { get; set; }
        public int Qty { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Total { get; set; }


        public bool? IsForAdendum { get; set; } = false;
        public string SiteCode { get; set; }


        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }



    }





      

}
