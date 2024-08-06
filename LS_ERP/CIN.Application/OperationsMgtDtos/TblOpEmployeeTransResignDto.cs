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

    [AutoMap(typeof(TblOpEmployeeTransResign))]
    public class TblOpEmployeeTransResignDto : AuditableEntityDto<long>
    {
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AttnDate { get; set; }
        public bool TR { get; set; }
        public bool R { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
     

    }
}
