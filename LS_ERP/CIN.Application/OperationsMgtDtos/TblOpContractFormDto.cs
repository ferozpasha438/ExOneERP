using AutoMapper;
using CIN.Application.SystemSetupDtos;
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
    [AutoMap(typeof(TblOpContractFormHead))]

    public class TblOpContractFormHeadDto
    {
      
        public long Id { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }



        public string TitleOfServiceEng { get; set; }
        public string CompanyDetailsEng { get; set; }          //includes <<company>>
        public string CustomerDetailsEng { get; set; }             //includes<<>customer>
        public string PreambleEng { get; set; }
        public string FirstPartyEng { get; set; }
        public string SecondPartyEng { get; set; }






        public string TitleOfServiceArb { get; set; }
        public string CompanyDetailsArb { get; set; }          //includes <<company>>
        public string CustomerDetailsArb { get; set; }             //includes<<>customer>
        public string PreambleArb { get; set; }
        public string FirstPartyArb { get; set; }
        public string SecondPartyArb { get; set; }







        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public int? ApprovedBy { get; set; }

    }
    [AutoMap(typeof(TblOpContractClausesToContractFormMap))]
    public class TblOpContractClausesToContractFormMapDto
    {

        public long Id { get; set; }

        public long ContractFormId { get; set; }



        public string ClauseTitleEng { get; set; }
        public string ClauseSubTitleEng { get; set; }
        public string ClauseDescriptionEng { get; set; }
        public string NumberEng { get; set; }

        public string ClauseTitleArb { get; set; }
        public string ClauseSubTitleArb { get; set; }
        public string ClauseDescriptionArb { get; set; }
        public string NumberArb { get; set; }
        public short SerialNumber { get; set; }
        public long MappingId { get; set; }
    }

    public class ContractFormDto
    {
        public TblOpContractFormHeadDto ContractFormHead { get; set; }
        public List<TblOpContractClausesToContractFormMapDto> ContractClauses { get; set; }

    }





}
