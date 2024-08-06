using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{

    #region CheckOpenItemMenthod

    public class CheckOpenItemMenthod : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
    }

    public class CheckOpenItemMenthodHandler : IRequestHandler<CheckOpenItemMenthod, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckOpenItemMenthodHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CheckOpenItemMenthod request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CheckOpenItemMenthod method start----");
            return await _context.FinSysFinanialSetups.AnyAsync(e => e.PaymentMethod == "OPIM");
        }
    }

    #endregion

    #region SingleItem

    public class GetFinSetup : IRequest<TblFinSysFinanialSetupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetFinSetupHandler : IRequestHandler<GetFinSetup, TblFinSysFinanialSetupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetFinSetupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinSysFinanialSetupDto> Handle(GetFinSetup request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetFinSetup method start----");
            var item = await _context.FinSysFinanialSetups.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .ProjectTo<TblFinSysFinanialSetupDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken) ?? new();
            Log.Info("----Info GetFinSetup method Ends----");
            return item;
        }
    }

    #endregion




    #region AcCodeSegmentSingleItem

    public class GetAcCodeSegment : IRequest<TblErpSysAcCodeSegmentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAcCodeSegmentHandler : IRequestHandler<GetAcCodeSegment, TblErpSysAcCodeSegmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAcCodeSegmentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysAcCodeSegmentDto> Handle(GetAcCodeSegment request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAcCodeSegment method start----");
            var item = await _context.AcCodeSegments.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new TblErpSysAcCodeSegmentDto
               {
                   CodeType1 = e.CodeType,
                   Segment1 = e.Segment,

               })
               .FirstOrDefaultAsync(cancellationToken) ?? new();
            Log.Info("----Info GetAcCodeSegment method Ends----");
            return item;
        }
    }

    #endregion


    #region CreateUpdate

    public class CreateFinancialsetup : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinSysFinanialSetupDto BranchDto { get; set; }
    }

    public class CreateFinancialsetupQueryQueryHandler : IRequestHandler<CreateFinancialsetup, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateFinancialsetupQueryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateFinancialsetup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateFinancialsetupQuery method start----");

                var obj = request.BranchDto;
                TblFinSysFinanialSetup FinanceSetUp = await _context.FinSysFinanialSetups.OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? new();

                FinanceSetUp.FYOpenDate = obj.FYOpenDate;
                FinanceSetUp.FYClosingDate = obj.FYClosingDate;
                FinanceSetUp.FYYear = obj.FYYear;
                FinanceSetUp.FinAcCatLen = obj.FinAcCatLen;
                FinanceSetUp.FinAcSubCatLen = obj.FinAcSubCatLen;
                FinanceSetUp.FinAcLen = obj.FinAcLen;
                //FinanceSetUp.FinAcFormat = obj.FinAcFormat;
                //FinanceSetUp.FinBranchPrefixLen = obj.FinBranchPrefixLen;
                FinanceSetUp.FinAllowNextYearTran = obj.FinAllowNextYearTran;
                FinanceSetUp.FinTranDateAsPostDate = obj.FinTranDateAsPostDate;
                FinanceSetUp.FInSysGenAcCode = obj.FInSysGenAcCode;
                FinanceSetUp.PaymentMethod = obj.PaymentMethod;
                FinanceSetUp.NumOfSeg = obj.NumOfSeg;
                FinanceSetUp.UserCostSeg = obj.UserCostSeg;
                FinanceSetUp.MinCutOffShortAmt = obj.MinCutOffShortAmt > 0 ? (-1) * obj.MinCutOffShortAmt : obj.MinCutOffShortAmt;
                FinanceSetUp.MaxCutOffOverAmr = obj.MaxCutOffOverAmr;
                FinanceSetUp.ArDistFlag = obj.ArDistFlag;

                if (FinanceSetUp.Id > 0)
                {
                    FinanceSetUp.ModifiedOn = obj.ModifiedOn;
                    _context.FinSysFinanialSetups.Update(FinanceSetUp);
                }
                else
                {
                    await _context.FinSysFinanialSetups.AddAsync(FinanceSetUp);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateBranch method Exit----");
                return ApiMessageInfo.Status(1, FinanceSetUp.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateFinancialsetupQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


    #region CreateAcCodeSegment

    public class CreateAcCodeSegment : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysAcCodeSegmentDto AcCodeDto { get; set; }
    }

    public class CreateAcCodeSegmentQueryQueryHandler : IRequestHandler<CreateAcCodeSegment, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAcCodeSegmentQueryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAcCodeSegment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateAcCodeSegmentQuery method start----");

                    TblErpSysAcCodeSegment segment1 = new();
                    //TblErpSysAcCodeSegment segment2 = new();
                    //TblErpSysAcCodeSegment segment3 = new();
                    var obj = request.AcCodeDto;

                    var list = await _context.AcCodeSegments.ToListAsync();
                    _context.AcCodeSegments.RemoveRange(list);


                    segment1.CodeType = obj.CodeType1;
                    segment1.Segment = obj.Segment1;
                    segment1.Type = "Account Code";
                    segment1.Len = 5;
                    segment1.Start = 0;
                    segment1.End = 4;

                    //segment2.CodeType = obj.CodeType2;
                    //segment2.Segment = obj.Segment2;
                    //segment2.Type = "Segment1";
                    //segment2.Len = 2;

                    //segment3.CodeType = obj.CodeType3;
                    //segment3.Segment = obj.Segment3;
                    //segment3.Type = "Segment2";
                    //segment3.Len = 2;


                    //if (segment1.Segment == 1)
                    //{
                    //    segment1.Start = 0;
                    //    segment1.End = 4;

                    //    //segment2.Start = 6;
                    //    //segment2.End = 7;

                    //    //segment3.Start = 9;
                    //    //segment3.End = 10;

                    //}

                    //else if (segment1.Segment == 2)
                    //{

                    //    segment2.Start = 0;
                    //    segment2.End = 1;

                    //    segment1.Start = 3;
                    //    segment1.End = 7;

                    //    segment3.Start = 9;
                    //    segment3.End = 10;
                    //}

                    //else if (segment1.Segment == 3)
                    //{

                    //    segment2.Start = 0;
                    //    segment2.End = 1;

                    //    segment3.Start = 3;
                    //    segment3.End = 4;

                    //    segment1.Start = 6;
                    //    segment1.End = 10;
                    //}

                    await _context.AcCodeSegments.AddAsync(segment1);

                    await _context.SaveChangesAsync();
                    Log.Info("----Info SaveUpdateBranch method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateAcCodeSegmentQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion
}
