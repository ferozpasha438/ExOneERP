using AutoMapper;
using CIN.Application;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SND;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SndDtos
{

    [AutoMap(typeof(TblSndAuthorities))]
    public class TblSndAuthoritiesDto : PrimaryKeyDto<int>
    {

        public int AppAuth { get; set; }  //reference from tblErpSysLogin loginId
        [StringLength(20)]
        public string BranchCode { get; set; }
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

    public class TblSndAuthoritiesPagedList : TblSndAuthoritiesDto
    {
        public string  UserName { get; set; }

    }
}
