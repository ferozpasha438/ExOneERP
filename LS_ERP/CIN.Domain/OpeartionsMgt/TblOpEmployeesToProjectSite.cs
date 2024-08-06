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
    [Table("tblOpEmployeesToProjectSite")]
    public class TblOpEmployeesToProjectSite : AuditableCreatedEntity<int>
    {
        public long EmployeeID { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        [StringLength(50)]
        public string EmployeeName { get; set; }      [StringLength(50)]
        public string EmployeeNameAr { get; set; }      //can Delete later
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
      
    }
}
