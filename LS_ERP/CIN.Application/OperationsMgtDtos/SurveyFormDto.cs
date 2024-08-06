using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    public class TblSndDefSurveyFormDto : TblSndDefSurveyFormHeadDto
    {
        public List<TblSndDefSurveyFormElementDto> ElementsList { get; set; }
    }
}
