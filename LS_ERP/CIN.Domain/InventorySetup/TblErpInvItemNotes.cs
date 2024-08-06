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

    [Table("tblErpInvItemNotes")]
    public class TblErpInvItemNotes : AutoActiveGenerateIdAuditableKey<int>
    {

        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Notes { get; set; }
        //[Column(TypeName = "date")]
        public DateTime NoteDates { get; set; }

    }
}
