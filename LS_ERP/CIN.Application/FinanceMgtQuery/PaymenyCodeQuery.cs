using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.SystemSetupDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{

    #region GetPagedList

    public class GetAccountlPaycodesList : IRequest<PaginatedList<FinDefAccountlPaycodesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAccountlPaycodesListHandler : IRequestHandler<GetAccountlPaycodesList, PaginatedList<FinDefAccountlPaycodesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountlPaycodesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<FinDefAccountlPaycodesDto>> Handle(GetAccountlPaycodesList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.FinAccountlPaycodes.AsNoTracking()

               .Where(e => //e.companyid == request.companyid &&
                             (e.FinPayCode.Contains(search) || e.FinPayName.Contains(search)))
               .OrderBy(request.Input.OrderBy)
               .Select(e => new FinDefAccountlPaycodesDto { Id = e.Id, FinPayName = e.FinPayName, FinPayCode = e.FinPayCode, IsActive = e.IsActive })
                 //.ProjectTo<FinDefAccountlPaycodesDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region GetSingleItem

    public class GetPayCode : IRequest<FinDefBankPayCodeCheckLeavesDto>
    {
        public UserIdentityDto User { get; set; }
        public string PayCode { get; set; }
    }

    public class GetPayCodeHandler : IRequestHandler<GetPayCode, FinDefBankPayCodeCheckLeavesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<FinDefBankPayCodeCheckLeavesDto> Handle(GetPayCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayCode method start----");
            var item = await _context.FinAccountlPaycodes.AsNoTracking()
                .Where(e => e.FinPayCode == request.PayCode)
              .Select(e => new FinDefBankPayCodeCheckLeavesDto
              {
                  FinPayCode = e.FinPayCode,
                  FinPayType = e.FinPayType,
                  FinBranchCode = e.FinBranchCode,

              })
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetPayCode method Ends----");
            return item;
        }
    }

    #endregion


    #region GetPayCodeById

    public class GetPayCodeById : IRequest<FinDefAccountlPaycodesDto>
    {
        public UserIdentityDto User { get; set; }
        public int PayCode { get; set; }
    }

    public class GetPayCodeByIdHandler : IRequestHandler<GetPayCodeById, FinDefAccountlPaycodesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayCodeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<FinDefAccountlPaycodesDto> Handle(GetPayCodeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayCodeById method start----");
            var item = await _context.FinAccountlPaycodes.AsNoTracking()
                .Where(e => e.Id == request.PayCode)
              .Select(e => new FinDefAccountlPaycodesDto
              {
                  Id = e.Id,
                  FinPayCode = e.FinPayCode,
                  FinPayType = e.FinPayType,
                  FinBranchCode = e.FinBranchCode,
                  FinPayName = e.FinPayName,
                  FinPayAcIntgrAC = e.FinPayAcIntgrAC,
                  FinPayPDCClearAC = e.FinPayPDCClearAC,
                  IsActive = e.IsActive,
                  UseByOtherBranches = e.UseByOtherBranches,
                  SystemGenCheckBook = e.SystemGenCheckBook,

              })
                  .FirstOrDefaultAsync(cancellationToken);

            var checkLeaves = await _context.FinBankCheckLeaves.FirstOrDefaultAsync(e => e.FinPayCode == item.FinPayCode);

            if (checkLeaves is not null)
            {
                item.CheckLeavePrefix = checkLeaves.CheckLeavePrefix;
                item.EndChkNum = checkLeaves.EndChkNum;
                item.StChkNum = checkLeaves.StChkNum;

            }

            Log.Info("----Info GetPayCodeById method Ends----");
            return item;
        }
    }

    #endregion



    #region GetSelectPaymentCodeList

    public class GetSelectPaymentCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectPaymentCodeListHandler : IRequestHandler<GetSelectPaymentCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectPaymentCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectPaymentCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectPaymentCodeList method start----");
            var item = await _context.FinAccountlPaycodes.AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Where(e => e.IsActive)
               .Select(e => new CustomSelectListItem { Text = e.FinPayCode, Value = e.FinPayName })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectPaymentCodeList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSelectPaymentCashCodeList

    public class GetSelectPaymentCashCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectPaymentCashCodeListHandler : IRequestHandler<GetSelectPaymentCashCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectPaymentCashCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectPaymentCashCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectPaymentCashCodeList method start----");
            var finPayType = ((short)PayCodeTypeEnum.Cash).ToString();
            var item = await _context.FinAccountlPaycodes.AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Where(e => e.IsActive && e.FinPayType == finPayType)
               .Select(e => new CustomSelectListItem { Text = e.FinPayCode, Value = e.FinPayName })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectPaymentCashCodeList method Ends----");
            return item;
        }
    }

    #endregion


    #region GetSelectPaymentBankCodeList

    public class GetSelectPaymentBankCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectPaymentBankCodeListHandler : IRequestHandler<GetSelectPaymentBankCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectPaymentBankCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectPaymentBankCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectPaymentBankCodeList method start----");
            var finPayType = ((short)PayCodeTypeEnum.Bank).ToString();

            var item = await _context.FinAccountlPaycodes.AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Where(e => e.IsActive && e.FinPayType == finPayType)
               .Select(e => new CustomSelectListItem { Text = e.FinPayCode, Value = e.FinPayName })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectPaymentBankCodeList method Ends----");
            return item;
        }
    }

    #endregion

    #region CreateUpdate

    public class CreateAccountBranchMapping : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public FinDefBankPayCodeCheckLeavesDto Input { get; set; }
    }

    public class CreatePaymentCodeQueryQueryHandler : IRequestHandler<CreateAccountBranchMapping, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePaymentCodeQueryQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAccountBranchMapping request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreatePaymentCodeQuery method start----");

                    var obj = request.Input;
                    TblFinDefAccountlPaycodes PayCode = await _context.FinAccountlPaycodes.FirstOrDefaultAsync(e => e.Id == request.Input.Id) ?? new();
                    PayCode.FinBranchCode = obj.FinBranchCode.ToUpper();
                    PayCode.FinPayType = obj.FinPayType;
                    PayCode.FinPayName = obj.FinPayName;
                    PayCode.FinPayAcIntgrAC = obj.FinPayAcIntgrAC;
                    PayCode.FinPayPDCClearAC = obj.FinPayPDCClearAC;
                    PayCode.IsActive = obj.IsActive;
                    PayCode.UseByOtherBranches = obj.UseByOtherBranches;
                    PayCode.SystemGenCheckBook = obj.SystemGenCheckBook;



                    if (PayCode.Id > 0)
                    {
                        PayCode.ModifiedOn = DateTime.Now;
                        _context.FinAccountlPaycodes.Update(PayCode);

                    }
                    else
                    {
                        PayCode.FinPayCode = obj.FinPayCode.ToUpper();
                        if (await _context.FinAccountlPaycodes.AnyAsync(e => e.FinPayCode == obj.FinPayCode))
                            return ApiMessageInfo.DuplicateInfo(nameof(obj.FinPayCode));

                        PayCode.CreatedOn = DateTime.Now;
                        await _context.FinAccountlPaycodes.AddAsync(PayCode);
                    }

                    await _context.SaveChangesAsync();

                    if (((int)PayCodeTypeEnum.Bank).ToString() == obj.FinPayType)
                    {
                        if (!await _context.FinBankCheckLeaves.AnyAsync(e => e.FinPayCode == PayCode.FinPayCode))
                        {
                            TblFinDefBankCheckLeaves CheckLeave = new()
                            {
                                FinPayCode = obj.FinPayCode.ToUpper(),
                                StChkNum = obj.StChkNum,
                                EndChkNum = obj.EndChkNum,
                                CheckLeavePrefix = obj.CheckLeavePrefix,
                                CreatedOn = DateTime.Now
                            };

                            await _context.FinBankCheckLeaves.AddAsync(CheckLeave);
                            await _context.SaveChangesAsync();
                        }

                        //TblFinDefBankCheckLeaves CheckLeave = new();
                        //if (PayCode is not null)
                        //    CheckLeave = await _context.FinBankCheckLeaves.FirstOrDefaultAsync(e => e.FinPayCode == PayCode.FinPayCode) ?? new();

                        //CheckLeave.StChkNum = obj.StChkNum;
                        //CheckLeave.FinPayCode = obj.FinPayCode;
                        //CheckLeave.CreatedOn = DateTime.Now;

                        //if (CheckLeave.FinPayCode is not null)
                        //    _context.FinBankCheckLeaves.Update(CheckLeave);
                        //else
                        //    await _context.FinBankCheckLeaves.AddAsync(CheckLeave);


                    }

                    await transaction.CommitAsync();

                    Log.Info("----Info SaveUpdateBranch method Exit----");
                    return ApiMessageInfo.Status(1, PayCode.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreatePaymentCodeQuery Method");
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
