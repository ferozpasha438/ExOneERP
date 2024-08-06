using AutoMapper;
using CIN.Application;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{

 [AutoMap(typeof(TblOpLogisticsandvehicle))]
    public class TblOpLogisticsandvehicleDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
[StringLength(20)]
public string VehicleNumber { get; set; }
[StringLength(200)]
public string VehicleNameInEnglish { get; set; }
[StringLength(200)]
public string VehicleNameInArabic { get; set; }

public decimal DailyFuelCost { get; set; }

public decimal DailyMiscCost { get; set; }

public decimal EstimatedDailyMaintenanceCost { get; set; }

public decimal OtherDailyOperationCost { get; set; }

public decimal TotalDailyServiceCost { get; set; }

public decimal DailyServicePrice { get; set; }

public decimal ValueofVehicle { get; set; }
public string Vehicletype { get; set; }

public decimal MinMargin { get; set; }
[StringLength(500)]
public string Remarks { get; set; }
    }
}
