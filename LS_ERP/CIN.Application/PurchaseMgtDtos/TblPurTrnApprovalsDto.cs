using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CIN.Domain.PurchaseMgt;

namespace CIN.Application.PurchaseMgtDtos
{
    [AutoMap(typeof(TblPurAuthorities))]
    public class TblPurTrnApprovalsDto : PrimaryKeyDto<int>
    {
        public string BranchCode { get; set; }
        [StringLength(5)]
        public string ServiceType { get; set; }         //ENQ,EST,PROJ
        [StringLength(20)]
        public string ServiceCode { get; set; }

        public int AppAuth { get; set; }
        [StringLength(500)]
        public string AppRemarks { get; set; }
        public bool IsApproved { get; set; }

    }
}
