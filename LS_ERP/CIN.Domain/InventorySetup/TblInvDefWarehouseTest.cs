namespace CIN.Domain.InventorySetup
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CIN.Domain.SystemSetup;
    [Table("tblInvDefWarehouseTest")]
    public class TblInvDefWarehouseTest: AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(10)]
        [Key]
        public string WHCode { get; set; }
        [StringLength(50)]
        [Required]
        public string WHName { get; set; }
        [StringLength(50)]
        public string WHDesc { get; set; }
        [StringLength(500)]
        [Required]
        public string WHAddress { get; set; }
        [Required]
        [StringLength(20)]
        public string WHCity { get; set; }
        [StringLength(50)]
        public string WHState { get; set; }
        [Required]
        [StringLength(50)]
        public string WHIncharge { get; set; }
        
        [StringLength(20)]
        public string WHBranchCode { get; set; }
        
        [StringLength(20)]
        public string InvDistGroup { get; set; }
        public bool WhAllowDirectPur { get; set; }
    }
}
