using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpContractTermsMapToProject")]
    public class TblOpContractTermsMapToProject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
 [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
        public string ContractTerm { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Created { get; set; }
        public int CreatedBy { get; set; }
        public bool? isLiabilityAndInsurance { get; set; }
        public bool? isTerminationClause { get; set; }

    }

    [Table("tblOpPaymentTermsToProject")]
    public class TblOpPaymentTermsMapToProject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
 [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }

        [StringLength(50)]
        public string Particular { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InstDate { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime? MonthStartDate { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime? MonthEndDate { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Created { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ContractForProject
    {
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
[StringLength(20)]
        public string SiteCode { get; set; }

        public List<TblOpPaymentTermsMapToProject> PaymentTerms { get; set; }
        public List<TblOpContractTermsMapToProject> ContractTerms { get; set; }
    }



}
