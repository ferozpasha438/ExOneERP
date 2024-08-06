using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("HRM_DEF_PayrollGroup")]
    public class HRM_DEF_PayrollGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PayrollGroupID { get; set; }
        [StringLength(200)]
        public string PayrollGroupName_EN { get; set; }
        [StringLength(200)]
        public string PayrollGroupName_AR { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
      
        public long CreatedBy { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
     
        public DateTime ModifiedDate { get; set; }
        public long? CountryID { get; set; }
        public long? CompanyID { get; set; }
        public long? SiteID { get; set; }
        public long? ProjectId { get; set; }
        public long? BusinessUnitID { get; set; }
        public long? DivisionID { get; set; }
        public long? BranchID { get; set; }
        public long? DepartmentID { get; set; }
        public bool? IsForAllEmployee { get; set; }
        public bool? IsForFutureEmployee { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartPayRollDate { get; set; }
        public short? CurrentPayRollMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndPayRollDate { get; set; }
        public short? CurrentPayRollYear { get; set; }

    }
}
