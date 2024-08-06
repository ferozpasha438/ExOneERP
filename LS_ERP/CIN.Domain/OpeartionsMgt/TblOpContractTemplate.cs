using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpContractTemplate")]
    public class TblOpContractTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public bool? IsForProject { get; set; } 
        public bool? IsForAddingSite { get; set; } 
        public bool? IsForAddingResources { get; set; }
        public bool? IsForRemovingResources { get; set; }
    }
[Table("tblOpContractClause")]
    public class TblOpContractClause
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ClauseTitleEng { get; set; }
        public string ClauseSubTitleEng { get; set; }
        public string ClauseDescriptionEng { get; set; }
        public string NumberEng { get; set; }

        public string ClauseTitleArb { get; set; }
        public string ClauseSubTitleArb { get; set; }
        public string ClauseDescriptionArb { get; set; }
        public string NumberArb { get; set; }


    }
    
    [Table("tblOpContractTemplateToContractClauseMap")]
    public class TblOpContractTemplateToContractClauseMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ContractTemplateId { get; set; }

        [ForeignKey(nameof(ContractTemplateId))]
        public TblOpContractTemplate SysTemplateId { get; set; }

           public long ContractClauseId { get; set; }

        [ForeignKey(nameof(ContractClauseId))]
        public TblOpContractClause SysContractClauseId { get; set; }

        public short SerialNumber { get; set; }

    }
}
