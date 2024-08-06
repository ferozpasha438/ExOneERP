using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.OpeartionsMgt;


namespace CIN.Application.OperationsMgtQuery
{

    

    #region CreateUpdateOpApprovals
    public class CreateOpApprovals : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOprTrnApprovalsDto Input { get; set; }
    }

    public class CreateOpApprovalsHandler : IRequestHandler<CreateOpApprovals, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOpApprovalsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOpApprovals request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateOpApprovals method start----");



                    var obj = request.Input;

                    //var branch = _context.CompanyBranches.SingleOrDefaultAsync(e=>e.BranchCode==request.User.BranchCode);

                    TblOprTrnApprovals OpApprovals = new();

                    OpApprovals.IsApproved = obj.IsApproved;
                    OpApprovals.ServiceCode = obj.ServiceCode;
                    OpApprovals.ServiceType = obj.ServiceType;
                    OpApprovals.AppRemarks = obj.AppRemarks;
                    OpApprovals.AppAuth = request.User.UserId;
                    OpApprovals.BranchCode = request.User.BranchCode;
                    OpApprovals.CreatedOn = DateTime.UtcNow;
                    await _context.TblOprTrnApprovalsList.AddAsync(OpApprovals);
                    await _context.SaveChangesAsync();
                   
                    /*Only For Survey Approval */
                    if (request.Input.ServiceType =="SUR") {
                        var appAuths = _context.TblOpAuthoritiesList.Where(e => e.CanApproveSurvey && e.BranchCode == obj.BranchCode).Count();
                        var approvals = _context.TblOprTrnApprovalsList.Where(e => e.ServiceType =="SUR" /*&& e.BranchCode == obj.BranchCode*/ && e.IsApproved && e.ServiceCode==obj.ServiceCode).Count();
                        var enquiry = await _context.OprEnquiries.FirstOrDefaultAsync(e=>e.EnquiryID.ToString()==obj.ServiceCode);
                        enquiry.IsApproved = approvals >= 1;// appAuths<=approvals;
                        _context.OprEnquiries.Update(enquiry);
                        await _context.SaveChangesAsync();
                    }
                    
                    
                    
                    
                    
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdateOpApprovals method Exit----");
                    return OpApprovals.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateOpApprovals Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region GetOpApprovalsByUserId
    public class GetOpApprovalsByUserId : IRequest<TblOprTrnApprovalsDto>
    {
        public UserIdentityDto User { get; set; }
       
    }

    public class GetOpApprovalsByUserIdHandler : IRequestHandler<GetOpApprovalsByUserId, TblOprTrnApprovalsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpApprovalsByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOprTrnApprovalsDto> Handle(GetOpApprovalsByUserId request, CancellationToken cancellationToken)
        {
            TblOprTrnApprovalsDto obj = new();
            var OpApprovals = await _context.TblOprTrnApprovalsList.AsNoTracking().ProjectTo<TblOprTrnApprovalsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.AppAuth == request.User.UserId);
            if (OpApprovals is null)
            {
                obj.Id = OpApprovals.Id;

                //obj.IsActive = false; ;
                //obj.AppLevel = 0;
               

               

            }
            else
                obj = OpApprovals;
            return obj;
        }
    }

    #endregion

    

    
    
    







    

}
