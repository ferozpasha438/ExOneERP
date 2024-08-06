using CIN.Domain.Payroll.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.Payroll.Management
{
    [Table("tblPRLTrnPayrollPackageComponent")]
    public class TblPRLTrnPayrollPackageComponent : PrimaryKey<int>
    {
        [ForeignKey(nameof(PackageCode))]
        public TblPRLTrnPayrollPackage TrnPackage { get; set; }
        [Required]
        [StringLength(20)]
        public string PackageCode { get; set; }

        /*Only Structed PayrollComponents will be considered.*/
        [ForeignKey(nameof(PayrollComponentCode))]
        public TblPRLSysPayrollComponent SysPayrollComponent { get; set; }
        [Required]
        [StringLength(20)]
        public string PayrollComponentCode { get; set; }

        /*If Payroll Component is based on formula then PayValue will be NULL.*/
        public decimal PayValue { get; set; }
    }
}
