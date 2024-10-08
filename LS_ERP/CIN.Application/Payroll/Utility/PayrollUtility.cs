using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.Utility
{
    public enum PayrollComponentType
    {
        Earning = 1,
        Deduction = 2,
        UnStructuredEarning = 3,
        UnStructuredDeduction = 4
    }

    public static class PayrollComponent
    {
        public static readonly string ABSENT = "ABSENT";
        public static readonly string BASIC = "BASIC";
    }
}
