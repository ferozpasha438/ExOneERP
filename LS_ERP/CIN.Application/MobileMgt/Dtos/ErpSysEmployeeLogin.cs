using AutoMapper;
using CIN.Domain.MobileMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.MobileMgt.Dtos
{
    [AutoMap(typeof(ErpSysEmployeeLogin))]
    public class ErpSysEmployeeLoginDto : PrimaryKeyDto<int>
    {
        public string EmployeeNumber { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; } = "SecurityGaurd";
        public bool IsSigned { get; set; } = false;
    }
}
