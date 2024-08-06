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
    [Table("tblParentsLogin")]
    public class TblParentsLogin: AuditableActiveEntity<int>
    {
        [StringLength(100)]
        public string RegisteredPhone { get; set; }
        [StringLength(100)]
        public string RegisteredEmail { get; set; }
        public string Password { get; set; }
        public DateTime InactiveOn { get; set; }
        public bool IsApprove { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime RegistedDate { get; set; }
        public bool CurrentLogin { get; set; }

    }

    [Table("tblWardDetails")]
    public class TblWardDetails: AuditableActiveEntity<int>
    {
        public int WardNumber { get; set; }
        public String Name { get; set; }

    }

    [Table("tblParentAddRequest")]
     public class TblParentAddRequest: AuditableActiveEntity<int>
    {
        public string RegisteredMobile { get; set; }
        public string RegisteredEmail { get; set; }
        public DateTime RequestDate { get; set; }
        public string StuAdmNum{ get; set; }
        public string Notes { get; set; }
        public bool IsAdded { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }

    }




}
