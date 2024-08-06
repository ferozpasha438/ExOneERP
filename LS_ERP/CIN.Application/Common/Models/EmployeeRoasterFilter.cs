using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.Common.Models
{
    public class EmployeeRoasterFilter
    {
        public string PayrollGroupCode { get; set; }
        public string BranchCode { get; set; }
        public string PayrollPeriodCode { get; set; }
    }
    public class RoasterColumn
    {
        public string RoasterDate { get; set; }
        public string RoasterDay { get; set; }
    }

    public class RoasterRow
    {
        public string RoasterDate { get; set; }
        public string ShiftCode { get; set; }
    }
}
