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

    [AutoMap(typeof(TblSndDefSurveyor))]
    public class TblSndDefSurveyorDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Display(Name = "Surveyor Code")]
        [StringLength(20)]
        public string SurveyorCode { get; set; }
        [StringLength(200)]
        public string SurveyorNameEng { get; set; }
        [StringLength(200)]
        public string SurveyorNameArb { get; set; }
        [StringLength(20)]
        public string Branch { get; set; }
    }



    public class SurveyorDto : TblSndDefSurveyorDto
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
    }
}