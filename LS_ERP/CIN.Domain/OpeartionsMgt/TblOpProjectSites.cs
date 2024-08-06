using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpProjectSites")]
    public class TblOpProjectSites
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
        [StringLength(20)]

        public string ProjectCode { get; set; }
        //[Required]
        [StringLength(20)]
        public string CustomerCode { get; set; }
[StringLength(20)]
        public string SiteCode { get; set; }
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



        [Column(TypeName = "date")]
        public DateTime? ActualEndDate { get; set; }



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



        public bool IsAdendum { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsClosed { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsInActive { get; set; }
        public bool IsActive { get; set; }
        public short? SiteWorkingHours { get; set; }

        public int? FileUploadBy { get; set; }
        public string FileUrl { get; set; }
    }
}
