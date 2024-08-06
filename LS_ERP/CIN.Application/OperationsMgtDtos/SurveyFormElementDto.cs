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
    [AutoMap(typeof(TblSndDefSurveyFormElement))]
    public class TblSndDefSurveyFormElementDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        
        [StringLength(20)]
        public string FormElementCode { get; set; }
        [StringLength(200)]
      
        public string ElementEngName { get; set; }
        [StringLength(200)]
        public string ElementArbName { get; set; }
        [StringLength(20)]
        public string ElementType { get; set; }
        [StringLength(500)]
        public string ListValueString { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}