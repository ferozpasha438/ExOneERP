using AutoMapper;
using CIN.Domain.FinanceMgt;
using CIN.Domain;
using CIN.Domain.GeneralLedger;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.GeneralLedgerDtos
{
    public class CreateBankReconciliationDto : TblFinTrnBankReconciliationDto
    {
        public List<TblFinTrnBankReconciliationItemDto> ItemList { get; set; }
    }


    [AutoMap(typeof(TblFinTrnBankReconciliation))]
    public class TblFinTrnBankReconciliationDto : PrimaryKey<int>
    {
        [StringLength(50)]
        public string SpVoucherNumber { get; set; }

        [StringLength(50)]
        public string VoucherNumber { get; set; }
        [StringLength(20)]
        public string PayCode { get; set; }
        public string BranchCode { get; set; }

        public DateTime Date { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }

        public string Narration { get; set; }
        public string BranchName { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool Posted { get; set; }
        public bool Void { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? CDate { get; set; }
    }

    [AutoMap(typeof(TblFinTrnBankReconciliationItem))]
    public class TblFinTrnBankReconciliationItemDto : PrimaryKey<int>
    {
        public int BankRecId { get; set; }
        public string BranchCode { get; set; }
        [StringLength(50)]
        public string FinAcCode { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }

        [StringLength(150)]
        public string Batch { get; set; }
        [StringLength(150)]
        public string Batch2 { get; set; }
        public int? CostAllocation { get; set; }
        [StringLength(50)]
        public string CostSegCode { get; set; }
        public string BranchName { get; set; }
        public string Batch2Name { get; set; }

        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }
    }

}
