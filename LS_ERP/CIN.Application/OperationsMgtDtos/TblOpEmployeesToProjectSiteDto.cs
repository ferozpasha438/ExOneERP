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

    [AutoMap(typeof(TblOpEmployeesToProjectSite))]
    public class TblOpEmployeesToProjectSiteDto : AuditableCreatedEntityDto<int>
    {
        public long EmployeeID { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; } //can delete later
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [StringLength(50)]
        public string EmployeeNameAr { get; set; }

        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
    }
public class OpEmployeesToProjectSiteDto : TblOpEmployeesToProjectSiteDto
    {
        public HRM_TRAN_EmployeeDto EmployeeData { get; set; }
    }

}
