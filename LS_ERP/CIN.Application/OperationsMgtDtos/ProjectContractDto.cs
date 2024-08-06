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

    [AutoMap(typeof(TblOpContractTermsMapToProject))]
    public class TblOpContractTermsMapToProjectDto
    {

        public long Id { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
        public string ContractTerm { get; set; }

        public DateTime? Created { get; set; }
        public int CreatedBy { get; set; }
        public bool? isLiabilityAndInsurance { get; set; }
        public bool? isTerminationClause { get; set; }
    }




    [AutoMap(typeof(TblOpPaymentTermsMapToProject))]
    public class TblOpPaymentTermsMapToProjectDto
    {

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
        public DateTime? InstDate { get; set; }



        //public DateTime? MonthStartDate { get; set; }
        //public DateTime? MonthEndDate { get; set; }

        public decimal Amount { get; set; }
        public DateTime? Created { get; set; }
        public int CreatedBy { get; set; }
    }




    public class ContractForProjectDto
    {
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }

        [StringLength(20)]
        public string BranchCode { get; set; }

        public List<TblOpPaymentTermsMapToProjectDto> PaymentTerms { get; set; }
        public List<TblOpContractTermsMapToProjectDto> ContractTerms { get; set; }
    }


     public class InputApproveContractFormDto
    {
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }

        public long  ContractFormHeadId { get; set; }
    }


    
}
