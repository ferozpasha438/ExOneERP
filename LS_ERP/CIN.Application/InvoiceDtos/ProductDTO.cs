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
    [AutoMap(typeof(TblTranDefProduct))]
    public class ProductDTO : PrimaryKeyDto<int>
    {        
        [Required]
        public string NameEN { get; set; }
        [Required]
        public string NameAR { get; set; }
        public int CompanyId { get; set; }
       // [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ProductTypeId { get; set; }

        //[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessageResourceType = typeof(ViewResources.Resource), ErrorMessageResourceName = "Invoice_Validation_InvalidNumber")]
        //[RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Required]
        public string UnitPrice { get; set; }
        //[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessageResourceType = typeof(ViewResources.Resource), ErrorMessageResourceName = "Invoice_Validation_InvalidNumber")]
       // [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Required]
        public string CostPrice { get; set; }
        [Required]
        public int UnitTypeId { get; set; }
       // [Required]
        public string Barcode { get; set; }
        public bool IsDefaultConfig { get; set; }

        public string UnitType { get; set; }
        //invoice properties
        public decimal Quantity { get; set; }
        public decimal TotalAmount { get; set; }

    }
    public class ProductDTO_Ln : ProductDTO
    {
        public string BtnCreat_Ln { get; set; }
        public string BtnEdit_Ln { get; set; }
    }
}
