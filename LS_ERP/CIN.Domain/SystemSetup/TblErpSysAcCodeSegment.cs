using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysAcCodeSegment")]
    public class TblErpSysAcCodeSegment : PrimaryKey<int>
    {
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string CodeType { get; set; }
        public short Segment { get; set; }
        public short Len { get; set; }
        public short Start { get; set; }
        public short End { get; set; }
    }
}
