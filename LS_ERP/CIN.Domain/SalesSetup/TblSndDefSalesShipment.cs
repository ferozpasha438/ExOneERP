
namespace CIN.Domain.SalesSetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblSndDefSalesShipment")]
    public class TblSndDefSalesShipment : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string ShipmentCode { get; set; }
        [StringLength(50)]
        public string ShipmentName { get; set; }
        [StringLength(50)]
        public string ShipmentDesc { get; set; }
        [StringLength(10)]
        public string ShipmentType { get; set; }

        
    }
}
