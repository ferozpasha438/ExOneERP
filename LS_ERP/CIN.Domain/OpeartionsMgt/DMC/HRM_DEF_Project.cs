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
    [Table("HRM_DEF_Project")]
    public class HRM_DEF_Project
    {
        [Key]
        public long ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName_EN { get; set; }
        public string ProjectName_AR { get; set; }
        public string ProjectDescription { get; set; }
        public bool? IsActive { get; set; }
        public int? IsSystem { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ProjectSiteID { get; set; }
        public string CustomerCode { get; set; }
        public string SiteCode { get; set; }
    }
}
