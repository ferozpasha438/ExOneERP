using AutoMapper;
using CIN.Domain;
using CIN.Domain.SalesSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos.Shared
{
    [AutoMap(typeof(TblInvDefSalesConfig))]
    public class TblInvDefSalesConfigDto : PrimaryKey<int>
    {
        public bool AutoGenCustCode { get; set; }
        public bool PrefixCatCode { get; set; }
        [StringLength(10)]
        public string NewCustIndicator { get; set; }
        public short CustLength { get; set; }
        public short CategoryLength { get; set; }

    }
}
