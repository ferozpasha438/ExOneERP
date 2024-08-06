using AutoMapper;
using CIN.Domain.FinanceMgt;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblFinDefMainAccounts))]
   
    public class TblFinDefMainAccountsDto : AutoGenerateIdKeyDto<int>
    {
        public string FinAcCode { get; set; }
        
        [StringLength(200)]
        public string FinAcName { get; set; }
        [StringLength(200)]
        public string FinAcDesc { get; set; }
        [StringLength(50)]
        public string FinAcAlias { get; set; }
        public bool FinIsPayCode { get; set; }

        [StringLength(10)]
        public string FinPayCodetype { get; set; }
        public bool FinIsIntegrationAC { get; set; }

        [StringLength(15)]
        public string Fintype { get; set; } //Reference  TypeCode
  
        [StringLength(20)]
        public string FinCatCode { get; set; } //Reference  tblFinDefAccountCategory FinCatCode
      
        [StringLength(20)]
        public string FinSubCatCode { get; set; } //Reference  tblFinDefAccountSubCategory FinSubCatCode        
        public short FinActLastSeq { get; set; }
      
        public DateTime? CreatedOn { get; set; }
    
        public DateTime? ModifiedOn { get; set; }

    }
}
