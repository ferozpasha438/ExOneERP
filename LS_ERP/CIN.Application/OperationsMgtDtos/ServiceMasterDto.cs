using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefServiceMaster))]
    public class TblSndDefServiceMasterDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
       
        [StringLength(20)]
        public string ServiceCode { get; set; }
        [StringLength(200)]
      
        public string ServiceNameEng { get; set; }
        [StringLength(200)]
        public string ServiceNameArb { get; set; }
        [StringLength(20)]
        public string SurveyFormCode { get; set; }

    }
}