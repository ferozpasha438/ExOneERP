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
    [AutoMap(typeof(TblSndDefServiceUnitMap))]
    public class TblSndDefServiceUnitMapDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [StringLength(50)]
        public string ServiceCode { get; set; }

   
       [StringLength(20)]

        public string UnitCode { get; set; }

       public decimal PricePerUnit { get; set; }


    }
}