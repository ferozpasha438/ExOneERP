using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysFileUpload")]
    public class TblErpSysFileUpload : PrimaryKey<int>
    {
        [StringLength(80)]
        public string SourceId { get; set; }

        [StringLength(20)]
        public string Type { get; set; }
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(80)]
        public string FileName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(124)]
        public string UploadedBy { get; set; }
        public bool Status { get; set; }
    }
}
