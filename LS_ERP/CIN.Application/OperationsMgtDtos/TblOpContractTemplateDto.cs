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
    [AutoMap(typeof(TblOpContractTemplate))]
    
    public class TblOpContractTemplateDto
    {

        public long Id { get; set; }
       




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

        public bool? IsForProject { get; set; } = false;
        public bool? IsForAddingSite { get; set; } = false;
        public bool? IsForAddingResources { get; set; } = false;
        public bool? IsForRemovingResources { get; set; } = false;


    }

    public class ContractTemplatesPaginationDto: TblOpContractTemplateDto
    {
        public bool CanEdit { get; set; }

    }



    [AutoMap(typeof(TblOpContractClause))]

    public class TblOpContractClauseDto
    {
        public long Id { get; set; }
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

    [AutoMap(typeof(TblOpContractTemplateToContractClauseMap))]
    public class TblOpContractTemplateToContractClauseMapDto
    {
       
        public long Id { get; set; }
        public long ContractTemplateId { get; set; }
        public long ContractClauseId { get; set; }
        public short SerialNumber { get; set; }


    }


    public class ContractFormTemplateDto
    {
        public long TemplateId { get; set; }
        public TblOpContractTemplateDto TemplateHead { get; set; }
        public List<TblOpContractClauseDto> Clauses { get; set; }

    }






    public class ContractFormTemplateElementsPaginationDto: TblOpContractClauseDto
    {



    }




}
