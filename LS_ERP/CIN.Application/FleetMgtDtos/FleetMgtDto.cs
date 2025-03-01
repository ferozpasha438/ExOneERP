﻿using AutoMapper;
using CIN.Domain.FleetMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.FleetMgtDtos
{
    [AutoMap(typeof(TblVehicleCompanyMaster))]
    public class VehicleCompanyMasterDto
    {
        public int Id { get; set; }
        public string VehicleCompany { get; set; }
        public string VehicleCompany_Ar { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblVehicleTypeMaster))]
    public class VehicleTypeMasterDto
    {
        public int Id { get; set; }
        public string VehicleType { get; set; }
        public string VehicleType_Ar { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblVehicleBrandMaster))]
    public class VehicleBrandMasterDto
    {
        public int Id { get; set; }
        public string VehicleCompany { get; set; }
        public string VehicleType { get; set; }
        public string Brand { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblDriverMaster))]
    public class DriverMasterDto
    {
        public int Id { get; set; }
        public string DriverName { get; set; }
        public string DriverName_Ar { get; set; }
        public string IqamaNumber { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime Validity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblRouteMaster))]
    public class RouteMasterDto
    {
        public int Id { get; set; }
        public string RouteCode { get; set; }
        [Required]
        public string RouteName { get; set; }
        public string RouteNameAr { get; set; }
        public string City { get; set; }
        public string RouteLongitude { get; set; }
        public string RouteLatitude { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblRoutePlanHeader))]
    public class RoutePlanHeaderDto
    {
        public RoutePlanHeaderDto()
        {
            RoutePlanDetailsRows = new();
        }
        public int Id { get; set; }
        [Required]
        public string RoutePlanCode { get; set; }
        public string RouteNameEn { get; set; }
        public string RouteNameAr { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public List<RoutePlanDetailsDto> RoutePlanDetailsRows { get; set; }
    }


    [AutoMap(typeof(TblRoutePlanDetails))]
    public class RoutePlanDetailsDto
    {
        public int Id { get; set; }
        [Required]
        public int RouteId { get; set; }
        [Required]
        public string RouteCode { get; set; }
        public string Flag { get; set; }
        
    }

    [AutoMap(typeof(TblVehicleMaster))]
    public class VehicleMasterDto
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string VehicleCompany { get; set; }
        public string VehicleType { get; set; }
        public string Brand { get; set; }
        public string ChassisNumber { get; set; }
        public string SeatingCapacity { get; set; }
        public string RegisteredRTORegion { get; set; }
        public string RegistrationAuthority { get; set; }
        public string VehicleValidityTill { get; set; }
        public string VehicleCondition { get; set; }
        public string VehicleOwnership { get; set; }
        public DateTime ProcurementLeasedOn { get; set; }
        public string CurrentBookValue { get; set; }
        public string AnnualLeaseValue { get; set; }
        public string SalvageBookValue { get; set; }
        public DateTime LeaseEndDate { get; set; }
        public string VehicleOwnerEnglish { get; set; }
        public string VehicleOwnerArabic { get; set; }
        public string MeterReadingOnProcurement { get; set; }
        public string CurrentMeterReading { get; set; }
        public DateTime VehicleNextMaintenanceDate { get; set; }
        public string EstimatedMileagePerKM { get; set; }
        public DateTime VehicleLastMaintenanceDate { get; set; }
        public string EstimatedServiceYear { get; set; }
        public string FuelType { get; set; }
        public string FuelTankCapacityInLitters { get; set; }
        public bool IsActive { get; set; }
        public bool IsVehicleGoodsCarrier { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }


    }

    [AutoMap(typeof(TblVehicleFuelEntry))]
    public class VehicleFuelEntryDto
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; }
        public string FuelType { get; set; }
        public string FuelQuantity { get; set; }
        public DateTime FuellingDate { get; set; }
        public string Driver { get; set; }
        public string DocumentNumber { get; set; }
        public string ReadingKM { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblAssignDrivers))]
    public class AssignDriversDto
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; }
        public String DriverName { get; set; }
        public DateTime AssignDate { get; set; }
        public string Remarks { get; set; }
        public string NotesForDriver { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblAssignRoutes))]
    public class AssignRoutesDto
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; }
        public string RoutePlan { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }


    [AutoMap(typeof(TblServiceCode))]
    public class ServiceCodeDto
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceType { get; set; }
        public string ServiceName_En { get; set; }
        public string ServiceName_Ar { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    [AutoMap(typeof(TblServiceProvider))]
    public class ServiceProviderDto
    {
        public int Id { get; set; }
        public string ServiceProviderCode { get; set; }
        public string LocationCity { get; set; }
        public string ServiceProviderName_En { get; set; }
        public string ServiceProviderName_Ar { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

}
