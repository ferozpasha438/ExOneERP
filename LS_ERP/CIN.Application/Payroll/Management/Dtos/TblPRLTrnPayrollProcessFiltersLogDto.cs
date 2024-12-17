using AutoMapper;
using CIN.Domain.Payroll.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.Management.Dtos
{
    [AutoMap(typeof(TblPRLTrnPayrollProcessFiltersLog))]
    public class TblPRLTrnPayrollProcessFiltersLogDto : AuditableEntityDto<int>
    {
        //Payroll Group
        [Required]
        [StringLength(20)]
        public string PayrollGroupCode { get; set; }

        //Branch
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; }

        [Required]
        [StringLength(20)]
        public string PayrollMonth { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReleased { get; set; }
    }

    public class PayrollProcessFiltersDto
    {
        public string PayrollGroupCode { get; set; }
        public string BranchCode { get; set; }
    }
}
