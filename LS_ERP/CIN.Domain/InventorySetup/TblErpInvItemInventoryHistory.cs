using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.InventorySetup
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.SystemSetup;

    [Table("tblErpInvItemInventoryHistory")]
    public class TblErpInvItemInventoryHistory : AutoActiveGenerateIdAuditableKey<int>
    {

        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        public string ItemCode { get; set; }




        [ForeignKey(nameof(WHCode))]
        public TblInvDefWarehouse InvWarehouses { get; set; }
        [StringLength(10)]
        public string WHCode { get; set; }


        public DateTime TranDate { get; set; }


        [StringLength(3)]
        [Column(TypeName = "varchar(3)")]
        public string TranType { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string TranNumber { get; set; }

        [StringLength(10)]
        public string TranUnit { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal TranQty { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal unitConvFactor { get; set; }



        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal TranTotQty { get; set; }



        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal TranPrice { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal ItemAvgCost { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar(50)")]
        public string TranRemarks { get; set; }
    }
}
