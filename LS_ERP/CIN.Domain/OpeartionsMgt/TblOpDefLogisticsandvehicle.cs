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
    [Table("tblOpLogisticsandvehicle")]
    public class TblOpLogisticsandvehicle : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [StringLength(20)]
        public string VehicleNumber { get; set; }
        [StringLength(200)]
        public string VehicleNameInEnglish { get; set; }
        [StringLength(200)]
        public string VehicleNameInArabic { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal DailyFuelCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal DailyMiscCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal EstimatedDailyMaintenanceCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal OtherDailyOperationCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal TotalDailyServiceCost { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal DailyServicePrice { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal ValueofVehicle { get; set; }
        public string Vehicletype { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal MinMargin { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
