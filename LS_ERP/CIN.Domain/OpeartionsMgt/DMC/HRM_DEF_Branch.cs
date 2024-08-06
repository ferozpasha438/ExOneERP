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
    [Table("HRM_DEF_Branch")]
    public class HRM_DEF_Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long   BranchID { get; set; }
        //[StringLength(50)]
        public string BranchName_EN { get; set; }
        //[StringLength(50)]
        public string BranchName_AR { get; set; }
      //  [StringLength(200)]
        public string BranchDescription { get; set; }
        public bool? IsActive { get; set; }
        public int? IsSystem { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
       // [StringLength(300)]
        public string? Remarks { get; set; }
        public long? CompanyID { get; set; }
        public long? BusinessUnitID { get; set; }
       // [StringLength(20)]
        public string BranchCode { get; set; }
    }
}
