using AutoMapper;
using CIN.Domain.SalesSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefCustomerCategory))]
   
    public class TblSndDefCustomerCategoryDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        [StringLength(20)]
        public string CustCatCode { get; set; }
        [StringLength(50)]
        public string CustCatName { get; set; }
        [StringLength(50)]
        public string CustCatDesc { get; set; }
        [StringLength(3)]
        public string CatPrefix { get; set; }
        public int LastSeq { get; set; }

    }
}
