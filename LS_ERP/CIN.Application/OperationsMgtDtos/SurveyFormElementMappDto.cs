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
    [AutoMap(typeof(TblSndDefSurveyFormElementsMapp))]
    public class TblSndDefSurveyFormElementsMappDto : AutoGenerateIdKeyDto<int>
    {


        [StringLength(20)]
        public string SurveyFormCode { get; set; }

        [StringLength(20)]
        public string FormElementCode { get; set; }
    }
}