using AutoMapper;
using CIN.Application.OperationsMgtDtos;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.MobileMgt.Dtos
{

    [AutoMap(typeof(HRM_TRAN_EmployeeTimeChart))]
    public class HRM_TRAN_EmployeeTimeChartDto
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string AttnFlag { get; set; }
        public long? ShiftId { get; set; }
        public string SiteCode { get; set; }
        public byte? ShiftNumber { get; set; }

    }

    [AutoMap(typeof(TblErpSysIncidentReport))]
    public class TblErpSysIncidentReportDto : PrimaryKeyDto<int>
    {
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(128)]
        public string ImagePath { get; set; }
        public string Base64Image { get; set; }
        public decimal? SiteGeoLatitude { get; set; } = 0;
        public decimal? SiteGeoLongitude { get; set; } = 0;
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class CheckGeoLocationDto
    {
        public int Id { get; set; }
        // [Required]
        // public string SiteCode { get; set; }        
        [Required]
        public decimal SiteGeoLatitude { get; set; }
        [Required]
        public decimal SiteGeoLongitude { get; set; }

        //  [Required]
        public decimal SiteGeoGain { get; set; }
    }

    public class LocationTrackingDto : CheckGeoLocationDto
    {
        public decimal SiteLocationNvMeter { get; set; }
        public decimal SiteLocationPvMeter { get; set; }
        public decimal SiteLocationExtraMeter { get; set; }
    }

    public class EmployeeMobileLoginResponseDto
    {
        public int MobileLoginId { get; set; }

        public string EmployeeNumber{ get; set; }
        public string EmployeeNameEn { get; set; }
        public string EmployeeNameAr { get; set; }
        public List<MobEmployeeScheduledSchiftsDto> ShiftsList { get; set; } = new();
        public string ErrorMessage { get; set; }
    }

    public class MobEmployeeScheduledSchiftsDto
    {
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string SiteNameEn { get; set; }
        public string SiteNameAr { get; set; }
        public decimal SiteGeoLatitude { get; set; }
        public decimal SiteGeoLongitude { get; set; }
        public decimal SiteGeoGain { get; set; }
        public int StandardDeviation { get; set; }


        public string ShiftCode { get; set; }
        public TimeSpan? ShiftInTime { get; set; }
        public TimeSpan? ShiftOutTime { get; set; }
        public TimeSpan? LogInTime { get; set; }
        public TimeSpan? LogOutTime { get; set; }

        public string SkillsetCode { get; set; }
        public string SkillsetNameEng { get; set; }
        public string SkillsetNameArb { get; set; }

        public bool IsLoginToShift { get; set; }
        public bool IsLogoutFromShift { get; set; }
        public long AttId { get; set; } = 0;


    }
}
