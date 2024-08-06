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
    [Table("HRM_DEF_Department")]
    public class HRM_DEF_Department
    {
        [Key]
        public long DepartmentID { get; set; }
        public string DepartmentName_EN { get; set; }
        public string DepartmentName_AR { get; set; }
        public string DepartmentDescription { get; set; }
        public long DivisionID { get; set; }
        public long CostCenterID { get; set; }
        public bool IsActive { get; set; }
        public int IsSystem { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
