using CIN.DB;
using CIN.Domain.HumanResource.ServiceRequest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestQuery
{
    public static class ServiceRequestExt
    {
        public static async Task<List<CustomSelectListItem>> GetApprovalUserWithOrder(this CINDBOneContext _context, bool isArab, string ServiceRequestType, int EmployeeId)
        {
            var contractInfo = await _context.EmployeeContracts.AsNoTracking().Select(e => new { e.EmployeeID, e.BranchCode }).FirstOrDefaultAsync(e => e.EmployeeID == EmployeeId);
            var approvalMatrix = _context.ServiceRequestApprovalAuthorityMatrix.Include(e => e.SysApprovalAuthority)
                .Include(e => e.TrnPersonalInformation).AsNoTracking()
                .Where(e => e.BranchCode == contractInfo.BranchCode && e.ServiceRequestTypeCode == ServiceRequestType)
                .OrderBy(e => e.Id);

            var empControlUserIds = _context.EmployeeControls.Where(e => e.IsUser == true).Select(e => new { e.UserId, e.EmployeeID });
            return await (from matrix in approvalMatrix
                          join emp in empControlUserIds
                          on matrix.ManagerEmployeeID equals emp.EmployeeID
                          select new CustomSelectListItem
                          {
                              Text = isArab ? matrix.SysApprovalAuthority.ApprovalAuthorityNameAr : matrix.SysApprovalAuthority.ApprovalAuthorityNameEn,
                              TextTwo = isArab ? string.Concat(matrix.TrnPersonalInformation.FirstNameAr + " ", matrix.TrnPersonalInformation.LastNameAr)
                                         : string.Concat(matrix.TrnPersonalInformation.FirstNameEn + " ", matrix.TrnPersonalInformation.LastNameEn),
                              IntValue = emp.UserId.Value
                          }).ToListAsync();
        }

        public static async Task<IQueryable<TblHRMTrnEmployeeServiceRequestAudit>> GetAuditApprovalInfo(this CINDBOneContext _context, int ServiceRequestId)
        {
            var auditInfo = _context.EmployeeServiceRequestAudits.Where(e => e.EmployeeServiceRequestID == ServiceRequestId).OrderByDescending(e => e.Id);
            var lastRejectedId = await auditInfo.Where(e => e.ActionID == (int)ProcessStage.Reject).Select(e => e.Id).FirstOrDefaultAsync();
            return auditInfo.Where(e => e.Id > lastRejectedId && e.ActionID == (int)ProcessStage.Approved).OrderByDescending(e => e.Id);
        }
    }
}
