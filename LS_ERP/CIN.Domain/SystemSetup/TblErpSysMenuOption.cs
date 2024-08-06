using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysMenuOption")]
   // [Index(nameof(MenuCode), Name = "IX_TblErpSysMenuOption_MenuCode", IsUnique = false)]
    public class TblErpSysMenuOption : AutoGenerateIdKey<int>
    {
        [StringLength(10)]
        [Key]
        public string MenuCode { get; set; }
        public sbyte Level1 { get; set; }
        public sbyte Level2 { get; set; }
        public sbyte Level3 { get; set; }
        [StringLength(40)]
        public string MenuNameEng { get; set; }
        [StringLength(40)]
        public string MenuNameArb { get; set; }
        public bool IsForm { get; set; }

        [StringLength(40)]
        public string Path { get; set; }
        [StringLength(10)]
        public string ModuleName { get; set; }

    }
}
