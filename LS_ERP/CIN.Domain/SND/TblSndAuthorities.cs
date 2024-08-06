using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SND
{
    [Table("tblSndAuthorities")]
    public class TblSndAuthorities : PrimaryKey<int>
    {
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; } //Reference  BranchCode

        [ForeignKey(nameof(AppAuth))]
        public TblErpSysLogin SysLogin { get; set; }
    
        public int AppAuth { get; set; } //Reference User Login Table
        public short AppLevel { get; set; }



        public bool CanCreateSndInvoice { get; set; }
        public bool CanEditSndInvoice { get; set; }
        public bool CanApproveSndInvoice { get; set; }
        public bool CanPostSndInvoice { get; set; }
        public bool CanSettleSndInvoice { get; set; }
        public bool CanVoidSndInvoice { get; set; }
        
       
         public bool CanCreateSndQuotation { get; set; }
        public bool CanEditSndQuotation { get; set; }
        public bool CanApproveSndQuotation { get; set; }
        public bool CanConvertSndQuotationToOrder { get; set; }
        public bool CanConvertSndQuotationToInvoice { get; set; }
        public bool CanReviseSndQuotation { get; set; }
        public bool CanVoidSndQuotation { get; set; }
        public bool CanConvertSndQuotationToDeliveryNote { get; set; }
        
        public bool CanConvertSndDeliveryNoteToInvoice { get; set; }


        public bool IsFinalAuthority { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }
    }
}
