using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("HRM_TRAN_Employee")]
    public class HRM_TRAN_Employee
    {
        [Key]
        public long EmployeeID { get; set; }
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [StringLength(50)]
        public string EmployeeName_AR { get; set; }
        [StringLength(5)]
        public string EmployeeNumber { get; set; }
        public long? CreatedBy { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }

    }
    
}
