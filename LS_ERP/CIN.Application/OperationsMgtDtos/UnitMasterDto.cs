using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefUnitMaster))]
    public class TblSndDefUnitMasterDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        
        [StringLength(20)]
        public string UnitCode { get; set; }
        [StringLength(200)]
      
        public string UnitNameEng { get; set; }
        [StringLength(200)]
        public string UnitNameArb { get; set; }
    }
}