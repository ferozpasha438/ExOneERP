using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeVisaInfo")]
    public class TblHRMTrnEmployeeAccountInfo : AuditableEntity<int>
    {
        //EmployeeNumber
        [ForeignKey(nameof(EmployeeNumber))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        [StringLength(5)]
        public string EmployeeNumber { get; set; }

        //Bank
        [ForeignKey(nameof(BankCode))]
        public TblHRMSysBank SysBank { get; set; }
        [Required]
        [StringLength(20)]
        public string BankCode { get; set; }

        //Branch or IFSC Code
        [ForeignKey(nameof(IFSCCode))]
        public TblHRMSysBankBranch SysBankBranch { get; set; }
        [Required]
        [StringLength(20)]
        public string IFSCCode { get; set; }

        //IBAN Number or Account Number
        [Required]
        [StringLength(50)]
        public string AccountNo { get; set; }

        //Account Holder
        [Required]
        [StringLength(100)]
        public string AccountHolder { get; set; }

        //Is Salary Account
        [Required]
        public bool IsSalaryAccount { get; set; }

        //Remarks
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
