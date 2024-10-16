﻿using AutoMapper;
using CIN.Domain.InvoiceSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceDtos
{
    [AutoMap(typeof(TblTranDefProductType))]
    public class ProductTypeDTO : PrimaryKeyDto<int>
    {        
        public int CompanyId { get; set; }
        [Required]
        public string NameEN { get; set; }
        [Required]
        public string NameAR { get; set; }
        public bool IsDefaultConfig { get; set; }

    }

    public class ProductTypeDTO_Ln : ProductTypeDTO
    {
        public string BtnCreat_Ln { get; set; }
        public string BtnEdit_Ln { get; set; }
    }
}
