using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    public class ServicesEnquiryFormDto : TblSndDefServiceEnquiryHeaderDto
    {
        public List<TblSndDefServiceEnquiriesDto> EnquiriesList { get; set; }
    }
}
