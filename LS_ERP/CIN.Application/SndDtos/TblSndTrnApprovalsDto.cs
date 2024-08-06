using AutoMapper;
using CIN.Application;
using CIN.Application.SndDtos.Comman;
using CIN.Domain.SND;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SNdDtos
{

    [AutoMap(typeof(TblSndTrnApprovals))]
    public class TblSndTrnApprovalsDto : PrimaryKeyDto<int>
    {
        


      
        public string BranchCode { get; set; }
      
        public short ServiceType { get; set; }       
        [StringLength(30)]
        public string ServiceCode { get; set; }

         public int? AppAuth { get; set; }
        [StringLength(500)]
        public string AppRemarks { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
