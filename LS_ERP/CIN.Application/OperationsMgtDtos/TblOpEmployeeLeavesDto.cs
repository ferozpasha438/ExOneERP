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

    [AutoMap(typeof(TblOpEmployeeLeaves))]
    public class TblOpEmployeeLeavesDto 
    {
        [Key]
        public long Id { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AttnDate { get; set; }
        public bool AL { get; set; }
        public bool EL { get; set; }
        public bool UL { get; set; }
        public bool SL { get; set; }
        public bool W { get; set; }     //withdraw
        public bool STL{ get; set; }     //studyLeave

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }

        [StringLength(20)]
        public string? ProjectCode { get; set; }
        [StringLength(20)]
        public string? SiteCode { get; set; }
        [StringLength(100)]
        public string? ShiftCode { get; set; }
    }

    public class LeaveId
    {
        public long Id { get; set; }

    }
}
