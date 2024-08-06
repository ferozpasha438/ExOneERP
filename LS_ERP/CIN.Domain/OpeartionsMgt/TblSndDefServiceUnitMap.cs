using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblSndDefServiceUnitMap")]
    public class TblSndDefServiceUnitMap : AutoActiveGenerateIdAuditableKey<int>
    {
        //[Key]
        //[StringLength(20)]
        //public string ServiceUnitMapCode { get; set; }


        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        //[Column(Order = 1)]
        //public int Id { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime? CreatedOn { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime? ModifiedOn { get; set; }
        //public bool IsActive { get; set; }

        [Column(Order = 1)]
        [Key]
        [ForeignKey(nameof(ServiceCode))]
        public TblSndDefServiceMaster SysServiceCode { get; set; }
      //  [Required]
       // [StringLength(50)]
        public string ServiceCode { get; set; }
        
        [ForeignKey(nameof(UnitCode))]
        public TblSndDefUnitMaster SysUnitCode { get; set; }
       // [Required]
        // [StringLength(20)]
        [Column(Order = 2)]
        [Key]
        public string UnitCode { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        [Required]
        public decimal PricePerUnit { get; set; }
    }
}
