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
   public class Adjustments
    {
        [Key]
        
        public decimal PositiveSum { get; set; }
        public decimal NegativeSum { get; set; }
        
    }
}
