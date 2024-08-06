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
    [Table("HRM_DEF_Site")]
    public class HRM_DEF_Site
    {
        [Key]
        public long SiteID { get; set; }
        public long? ProjectID { get; set; }
       
        public string ProjectCode { get; set; }
        public string SiteName_EN { get; set; }
        public string SiteName_AR { get; set; }
        public string SiteDescription { get; set; }
        public long? BranchID { get; set; }
        public string SiteCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystem { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CityCode { get; set; }
    }
}
