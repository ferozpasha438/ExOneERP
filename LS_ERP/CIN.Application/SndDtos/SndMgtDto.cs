using AutoMapper;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SND;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.SndDtos
{
    #region For Binding To PayCode & CheckLeave

    [AutoMap(typeof(TblSndDefAccountlPaycodes))]
    public class FinDefAccountlPaycodesDto : PrimaryKeyDto<int>
    {
        [StringLength(20)]
        public string SndPayCode { get; set; }
        [StringLength(20)]
        public string SndBranchCode { get; set; }//Reference  BranchCode
        [Required]
        [StringLength(10)]
        public string SndPayType { get; set; }
        //  [Required]
        [StringLength(50)]
        public string SndPayName { get; set; }
        [StringLength(50)]
        public string SndPayAcIntgrAC { get; set; }//Reference FinAcCode
        [StringLength(50)]
        public string SndPayPDCClearAC { get; set; }//Reference FinAcCode
        public bool IsActive { get; set; }
        public bool UseByOtherBranches { get; set; }
        public bool SystemGenCheckBook { get; set; }


        [Required]
        public int StChkNum { get; set; }
        [Required]
        public int EndChkNum { get; set; }
        [StringLength(10)]
        public string CheckLeavePrefix { get; set; }

    }



    #endregion
}
