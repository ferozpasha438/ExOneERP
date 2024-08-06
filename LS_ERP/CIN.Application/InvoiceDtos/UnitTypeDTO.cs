using AutoMapper;
using CIN.Domain.InvoiceSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceDtos
{
    [AutoMap(typeof(TblTranDefUnitType))]
    public class UnitTypeDTO : PrimaryKeyDto<int>
    {       
        public int? CompanyId { get; set; }
        //[Required(ErrorMessageResourceType = typeof(ViewResources.Resource), ErrorMessageResourceName = "Invoice_Validation_Required")]
        [Required]
        public string NameEN { get; set; }        
        [Required]
        public string NameAR { get; set; }
        public bool IsDefaultConfig { get; set; }

    }

    public class UnitTypeDTO_Ln : UnitTypeDTO
    {
        public string BtnCreat_Ln { get; set; }
        public string BtnEdit_Ln { get; set; }
    }
}
