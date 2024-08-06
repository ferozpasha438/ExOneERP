using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysUserSiteMapping")]
    public class TblErpSysUserSiteMapping : PrimaryKey<int>
    {
        [StringLength(50)]
        public string LoginId { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; }

        public long? EmployeeID { get; set; }
    }
}
