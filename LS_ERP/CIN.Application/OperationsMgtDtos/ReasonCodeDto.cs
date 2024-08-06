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

    [AutoMap(typeof(TblOpReasonCode))]
    public class TblOpReasonCodeDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {
        public string ReasonCode { get; set; }

        public string ReasonCodeNameEng { get; set; }
        public string ReasonCodeNameArb { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionArb { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public bool? IsForCustomerVisit { get; set; } = false;
        public bool? IsForCustomerComplaint { get; set; } = false;
    }
}
