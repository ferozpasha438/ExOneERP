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

    [AutoMap(typeof(TblOprTrnApprovals))]
    public class TblOprTrnApprovalsDto : PrimaryKeyDto<int>
    {

      
        public string BranchCode { get; set; }
        [StringLength(10)]
        public string ServiceType { get; set; }         //ENQ,EST,PROJ
        [StringLength(30)]
        public string ServiceCode { get; set; }

        public int AppAuth { get; set; }
        [StringLength(500)]
        public string AppRemarks { get; set; }
        public bool IsApproved { get; set; }

    }
}
