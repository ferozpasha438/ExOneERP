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
    [Table("TblWebStudentRegistration")]
    public class TblWebStudentRegistration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Key]
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string GenderName { get; set; }
        public string DateOfBirth { get; set; }
        public string Grade { get; set; }
        public string City { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public string FatherPhoneNumber { get; set; }

        public string MotherPhoneNumber { get; set; }

        public int EnglishFluencyLevel { get; set; }

        public string Remarks { get; set; }

        public Boolean IsyourchildPottytrained { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }
}
