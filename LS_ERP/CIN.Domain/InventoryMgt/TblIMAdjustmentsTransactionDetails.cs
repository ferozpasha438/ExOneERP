using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.InventoryMgt
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("tblIMAdjustmentsTransactionDetails")]
    public class TblIMAdjustmentsTransactionDetails : AutoActiveGenerateIdAuditableKey<int>
    {



        [StringLength(20)]
        public string TranNumber { get; set; }

        public string SNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime TranDate { get; set; }

        [StringLength(10)]
        public string TranLocation { get; set; }

        [StringLength(10)]
        public string TranToLocation { get; set; }

        [StringLength(10)]
        public string TranType { get; set; }

        [StringLength(20)]
        public string TranItemCode { get; set; }

        [StringLength(25)]
        public string TranBarcode { get; set; }


        [StringLength(100)]
        public string TranItemName { get; set; }

        [StringLength(100)]
        public string TranItemName2 { get; set; }

        [Column(TypeName = "decimal(8,3)")]
        public decimal TranItemQty { get; set; }

        [StringLength(10)]
        public string TranItemUnit { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal TranUOMFactor { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal TranItemCost { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal TranTotCost { get; set; }

        [StringLength(25)]
        public string ItemAttribute1 { get; set; }

        [StringLength(25)]
        public string ItemAttribute2 { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        [StringLength(50)]
        public string INVAcc { get; set; }

        [StringLength(50)]
        public string INVADJAcc { get; set; }
    }
}
