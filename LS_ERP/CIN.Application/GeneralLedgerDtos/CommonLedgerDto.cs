using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.GeneralLedgerDtos
{

    public class CommonBalanceDto
    {
        public decimal? TotalDrAmount { get; set; }
        public decimal? TotalCrAmount { get; set; }
        public decimal? TotalBalance { get; set; }
    }

    public class CommonLedgerDto : PrimaryKeyDto<int>
    {
        public string CostAllocationName { get; set; }
        public int? CostAllocation { get; set; }
        [StringLength(50)]
        public string CostSegCode { get; set; }
        public string CostSegCodeName { get; set; }
        public bool IsDepartment { get; set; }
    }

    public class CommonDataLedgerDto : PrimaryKeyDto<int>
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyAddress { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LogoURL { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string VATNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PoBox { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }



}
