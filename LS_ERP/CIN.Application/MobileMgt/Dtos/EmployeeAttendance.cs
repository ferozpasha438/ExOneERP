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
   
    public class InputEnterEmployeeAttendanceThroughMobileDto : TblOpEmployeeAttendanceDto
    {
        
        public string Base64Image { get; set; }
        public string WebRootForAttendanceImages { get; set; }

    }
    public class InputEmployeeLogoutDto
    {
        public long AttnId { get; set; }
        public string LogoutTime { get; set; } = "00:00";
        public string Base64Image { get; set; }
        public string WebRootForAttendanceImages { get; set; }
    }
}
