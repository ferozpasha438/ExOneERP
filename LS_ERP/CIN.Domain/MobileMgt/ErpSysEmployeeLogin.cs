using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.MobileMgt
{
     [Table("tblErpSysEmployeeLogin")]
    [Index(nameof(EmployeeNumber), Name = "IX_tblErpSysEmployeeLogin_EmployeeNumber", IsUnique = true)]
    public class ErpSysEmployeeLogin : PrimaryKey<int>
    {
        [Required]
        public string EmployeeNumber { get; set; }
        [StringLength(128)]
        [Required]
        public string Password { get; set; }

        public string LoginType { get; set; } = "SecurityGaurd";
        public bool IsSigned { get; set; }


    }
}
