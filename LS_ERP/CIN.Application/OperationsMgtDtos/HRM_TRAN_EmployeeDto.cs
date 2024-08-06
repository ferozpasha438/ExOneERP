using AutoMapper;
using CIN.Application.Common;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(HRM_TRAN_Employee))]
    public class HRM_TRAN_EmployeeDto 
    {
       
        public long EmployeeID { get; set; }
        [StringLength(50)]
        public string EmployeeName { get; set; }
 [StringLength(50)]
        public string EmployeeName_AR { get; set; }
        [StringLength(5)]
        public string EmployeeNumber { get; set; }

        public long?CreatedBy { get; set; }
        public DateTime?CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }

    }



    public class EmployeeDtoForReports:HRM_TRAN_EmployeeDto
    {
        public DateTime? LastAttendedDay { get; set; }

    }
    //[AutoMap(typeof(DMC_TRAN_Employee))]
    //public class DMC_TRAN_EmployeeDto 
    //{

    //    public long EmployeeID { get; set; }
    //    [StringLength(200)]
    //    public string EmployeeName { get; set; }
    //    [StringLength(5)]
    //    public string EmployeeNumber { get; set; }

    //    public int? CreatedBy { get; set; }
    //    public DateTime? CreatedDate { get; set; }
    //    public int? ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public bool IsActive { get; set; }

    //}


    public class EmployeePrimarySiteLogsDto {
        public long PrimarySiteLogId { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameAr { get; set; }
        public string PrimarySiteCode { get; set; }
        public string PrimarySiteName { get; set; }
        public string PrimarySiteNameAr { get; set; }
        public DateTime? LastTransferDate { get; set; }
    }

  


    public class PaginationParametersDto: PaginationFilterDto
    {
               public ParametersDto Parameters { get; set; }

    }

    public class ParametersDto
    {
        public string EmployeeNumber { get; set; } = string.Empty;
    }

   
}
