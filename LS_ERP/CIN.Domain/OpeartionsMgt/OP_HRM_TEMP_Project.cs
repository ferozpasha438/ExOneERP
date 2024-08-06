using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("OP_HRM_TEMP_Project")]
    public class OP_HRM_TEMP_Project : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ProjectCode { get; set; }
        //[Required]
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [Required]
        [StringLength(200)]
        public string ProjectNameEng { get; set; }
        [StringLength(200)]
        public string ProjectNameArb { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        public bool IsResourcesAssigned { get; set; }
        public bool IsMaterialAssigned { get; set; }
        public bool IsLogisticsAssigned { get; set; }
        public bool IsShiftsAssigned { get; set; }
        public bool IsExpenceOverheadsAssigned { get; set; }
        public bool IsEstimationCompleted { get; set; }
        public bool IsSkillSetsMapped { get; set; }
        public bool IsConvertedToProposal { get; set; }
        public bool IsConvrtedToContract { get; set; }

        public string BranchCode { get; set; }

        public int? FileUploadBy { get; set; }
        public string FileUrl { get; set; }
    }
}
