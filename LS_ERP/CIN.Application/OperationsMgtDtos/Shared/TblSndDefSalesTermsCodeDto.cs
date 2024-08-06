using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SalesSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefSalesTermsCode))]
   
    public class TblSndDefSalesTermsCodeDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        public string SalesTermsCode { get; set; }
        [StringLength(50)]
        public string SalesTermsName { get; set; }
        [StringLength(50)]
        public string SalesTermsDesc { get; set; }
        public sbyte SalesTermsDueDays { get; set; }
        public sbyte SalesTermDiscDays { get; set; }

    }















}
