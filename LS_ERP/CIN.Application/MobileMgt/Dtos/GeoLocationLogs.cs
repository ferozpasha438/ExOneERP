using AutoMapper;
using CIN.Application.OperationsMgtDtos;
using CIN.Domain.MobileMgt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.MobileMgt.Dtos
{
   
    public class TblMobMgtGeoLocationLogsDto :  PrimaryKeyDto<long>
    {

        public long AttnId { get; set; }
        public bool IsGeofenceOut { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Remarks { get; set; }

    }
  
}
