using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefCenters")]
    public class TblFinDefCenters : PrimaryKey<int>
    {
        [ForeignKey(nameof(FinCenterCode))]
        public TblFinDefMainAccounts FinMainAccounts { get; set; }
        [StringLength(50)]
        public string FinCenterCode { get; set; } //Reference FinAcCode
        [StringLength(50)]
        public string FinCenterName { get; set; }
        [StringLength(10)]
        public string FinCenterType { get; set; } //Cost/Profit/NA

    }
}
