using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN.Domain;

namespace CIN.Domain.SchoolMgt
{
    [Table("tblParentMyGallery")]
    public class TblParentMyGallery
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string RegisterMobile { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public string Path { get; set; }
        public bool IsVedio { get; set; }
    }
}
