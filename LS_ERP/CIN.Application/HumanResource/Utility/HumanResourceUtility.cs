using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.Utility
{
    public static class EmployeeStatus
    {
        public static readonly string ACTIVE = "ACTIVE";
        public static readonly string QUIT = "QUIT";
        public static readonly string VACATION = "VACATION";
    }

    public static class ServiceRequestType
    {
        public static readonly string LeaveServiceRequest = "VAC";
        public static readonly string VacationServiceRequest = "VACA";
    }
}
