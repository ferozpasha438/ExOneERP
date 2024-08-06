using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnCustomerWallet")]
    public class TblFinTrnCustomerWallet : PrimaryKey<int>
    {
        [StringLength(20)]
        public string CustCode { get; set; }
        [StringLength(10)]
        public string Source { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal AdvAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18, 3)")]
        public decimal AppliedAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18, 3)")]
        public decimal PostedAmount { get; set; } = 0;
        public int CreatedBy { get; set; }
    }
}
