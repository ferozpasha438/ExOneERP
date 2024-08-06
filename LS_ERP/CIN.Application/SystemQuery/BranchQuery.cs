using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SystemSetupDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SystemQuery
{

    #region CheckBranchCode

    public class CheckBranchCode : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class CheckBranchCodeHandler : IRequestHandler<CheckBranchCode, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckBranchCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CheckBranchCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CheckBranchCode method start----");
            return await _context.CompanyBranches.AnyAsync(e => e.BranchCode == request.Input);
        }
    }

    #endregion

    #region GetBranchByBranchCode

    public class GetBranchByBranchCode : IRequest<CustomSelectListItem>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBranchByBranchCodeHandler : IRequestHandler<GetBranchByBranchCode, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchByBranchCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSelectListItem> Handle(GetBranchByBranchCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchByBranchCode method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.BranchCode == request.Input)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchAddress })
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetBranchByBranchCode method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSelectBranchCodeList

    public class GetSelectBranchCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectBranchCodeListHandler : IRequestHandler<GetSelectBranchCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectBranchCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectBranchCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectBranchCodeList method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchCode, Value = e.BranchCode, TextTwo = e.BranchName, })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectBranchCodeList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetBranchInfoByBranchCode

    public class GetBranchInfoByBranchCode : IRequest<TblErpSysCompanyBranchDto>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBranchInfoByBranchCodeHandler : IRequestHandler<GetBranchInfoByBranchCode, TblErpSysCompanyBranchDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchInfoByBranchCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysCompanyBranchDto> Handle(GetBranchInfoByBranchCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchInfoByBranchCode method start----");
            try
            {
                var item = await _context.CompanyBranches.AsNoTracking()
                   .Where(e => e.BranchCode == request.Input)
                   .OrderByDescending(e => e.Id)
                  .ProjectTo<TblErpSysCompanyBranchDto>(_mapper.ConfigurationProvider)
                     .FirstOrDefaultAsync(cancellationToken);
                Log.Info("----Info GetBranchInfoByBranchCode method Ends----");
                return item;

            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                return null;
            }
        }
    }

    #endregion

    #region GetBranchInfoByBranchCodeForSchoolInvoice

    public class GetBranchInfoByBranchCodeForSchoolInvoice : IRequest<ErpSysCompanyBranchDto>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBranchInfoByBranchCodeForSchoolInvoiceHandler : IRequestHandler<GetBranchInfoByBranchCodeForSchoolInvoice, ErpSysCompanyBranchDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchInfoByBranchCodeForSchoolInvoiceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ErpSysCompanyBranchDto> Handle(GetBranchInfoByBranchCodeForSchoolInvoice request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchInfoByBranchCode method start----");
            try
            {
                var item = await _context.CompanyBranches.AsNoTracking()
                   .Where(e => e.BranchCode == request.Input)
                   .OrderByDescending(e => e.Id)
                   .Select(x=>new ErpSysCompanyBranchDto 
                       { 
                         AccountNumber=x.AccountNumber,
                         AuthorityName=x.AuthorityName,
                         BankName=x.BankName,
                         BankNameAr=x.BankNameAr,
                         State=x.State,
                         BranchAddress=x.BranchAddress,
                         BranchAddressAr=x.BranchAddressAr,
                         BranchCode=x.BranchCode,
                         BranchName=x.BranchName,
                         City=x.City,
                         CompanyId=x.CompanyId,
                         CompanyName=x.SysCompany.CompanyName,
                         GeoLocLatitude=x.GeoLocLatitude,
                         GeoLocLongitude=x.GeoLocLongitude,
                         Iban=x.Iban,
                         Id=x.Id,
                         IsActive=x.IsActive,
                         Mobile=x.Mobile,
                         Phone=x.Phone,
                         Remarks=x.Remarks,
                         ZoneId=x.ZoneId
                       })
                     .FirstOrDefaultAsync();
                Log.Info("----Info GetBranchInfoByBranchCode method Ends----");
                return item;

            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                return null;
            }
        }
    }

    #endregion


    #region GetSelectBranchNameCodeList

    public class GetSelectBranchNameCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectBranchNameCodeListHandler : IRequestHandler<GetSelectBranchNameCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectBranchNameCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectBranchNameCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectBranchNameCodeList method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectBranchNameCodeList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSelectSysBranchList

    public class GetSelectSysBranchList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSysBranchListHandler : IRequestHandler<GetSelectSysBranchList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSysBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSysBranchList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectSysBranchList method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.IsActive && (e.BranchName.Contains(request.Input) || e.BranchCode.Contains(request.Input)))
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectSysBranchList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSelectSysBranchListByComId

    public class GetSelectSysBranchListByComId : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public int Input { get; set; }
    }

    public class GetSelectSysBranchListByComIdHandler : IRequestHandler<GetSelectSysBranchListByComId, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSysBranchListByComIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSysBranchListByComId request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectSysBranchListByComId method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.CompanyId == request.Input && e.IsActive)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectSysBranchListByComId method Ends----");
            return item;
        }
    }

    #endregion

    #region GetPagedList

    public class GetBranchList : IRequest<PaginatedList<TblErpSysCompanyBranchDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetBranchListHandler : IRequestHandler<GetBranchList, PaginatedList<TblErpSysCompanyBranchDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblErpSysCompanyBranchDto>> Handle(GetBranchList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.CompanyBranches.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.BranchCode.Contains(search) || e.BranchName.Contains(search) ||
                                e.AuthorityName.Contains(search) || e.Mobile.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblErpSysCompanyBranchDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region SingleItem

    public class GetBranch : IRequest<TblErpSysCompanyBranchDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetBranchHandler : IRequestHandler<GetBranch, TblErpSysCompanyBranchDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysCompanyBranchDto> Handle(GetBranch request, CancellationToken cancellationToken)
        {
            var item = await _context.CompanyBranches.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblErpSysCompanyBranchDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            item.CompanyName = (await _context.Companies.FirstOrDefaultAsync(e => e.Id == item.CompanyId))?.CompanyName ?? string.Empty;
            return item;
        }
    }

    #endregion


    #region CreateUpdate

    public class CreateBranch : IRequest<(string Message, int BranchId)>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysCompanyBranchDto Input { get; set; }
    }

    public class CreateBranchQueryHandler : IRequestHandler<CreateBranch, (string msg, int branchId)>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateBranchQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(string msg, int branchId)> Handle(CreateBranch request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info SaveUpdateBranchBranchs method start----");

                    var obj = request.Input;
                    TblErpSysCompanyBranch Branch = new();
                    if (obj.Id > 0)
                        Branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    if (string.IsNullOrWhiteSpace(obj.CompanyName))
                        return (ApiMessageInfo.Failed, 0);

                    var company = await _context.Companies.FirstOrDefaultAsync(e => e.CompanyName == obj.CompanyName);

                    Branch.BranchName = obj.BranchName;
                    Branch.CompanyId = company.Id;
                    Branch.ZoneId = obj.ZoneId;
                    Branch.BranchAddress = obj.BranchAddress;
                    Branch.Phone = obj.Phone;
                    Branch.Mobile = obj.Mobile;
                    Branch.AuthorityName = obj.AuthorityName;
                    Branch.City = obj.City;
                    Branch.State = obj.State;
                    Branch.BankName = obj.BankName;
                    Branch.BankNameAr = obj.BankNameAr;
                    Branch.BranchAddressAr = obj.BranchAddressAr;
                    Branch.AccountNumber = obj.AccountNumber;
                    Branch.Iban = obj.Iban;
                    Branch.IsActive = obj.IsActive;

                    if (obj.Id > 0)
                    {
                        _context.CompanyBranches.Update(Branch);
                    }
                    else
                    {

                        Branch.BranchCode = obj.BranchCode.Trim().ToUpper();
                        if (await _context.CompanyBranches.AnyAsync(e => e.BranchCode == obj.BranchCode))
                            return (ApiMessageInfo.Duplicate(nameof(obj.BranchCode)), 0);

                        await _context.CompanyBranches.AddAsync(Branch);


                        bool alreadyExists = await _context.Sequences.AnyAsync(e => e.BranchCode == Branch.BranchCode);
                        if (!alreadyExists)
                        {
                            TblSequenceNumberSetting sequenceSetting = new()
                            {
                                BranchCode = Branch.BranchCode,
                                InvoiceSeq = 0,
                                CreditSeq = 0,
                                VoucherSeq = 0,
                                ApVoucherSeq = 0,
                                JvVoucherSeq = 0,
                                CvVoucherSeq = 0,
                                BvVoucherSeq = 0,
                                ArPaymentNumber = 0,
                                ApPaymentNumber = 0,
                                PONumber = 0,
                                PRNumber = 0,
                                GRNNumber = 0,
                                SDQuoteNumber = 0,
                                SDInvoiceNumber = 0,
                                SDOrderNumber = 0,
                                SDDeliveryNumber = 0,
                                SDInvRetNumber = 0,
                                InvCredSeq = 0,
                                VendCreditSeq = 0,
                            };
                            await _context.Sequences.AddAsync(sequenceSetting);
                        }

                    }
                    await _context.SaveChangesAsync();


                    TblFinDefAccountBranches acBranch = await _context.FinAccountBranches.FirstOrDefaultAsync(e => e.FinBranchCode == obj.BranchCode) ?? new();

                    acBranch.FinBranchPrefix = string.Empty;
                    acBranch.FinBranchName = obj.BranchName;
                    acBranch.FinBranchDesc = obj.Remarks;
                    acBranch.FinBranchAddress = obj.BranchAddress;
                    acBranch.FinBranchType = "1";
                    acBranch.FinBranchDesc = " ";
                    acBranch.FinBranchIsActive = true;

                    if (acBranch.Id > 0)
                    {
                        _context.FinAccountBranches.Update(acBranch);
                    }
                    else
                    {
                        acBranch.FinBranchCode = obj.BranchCode.ToUpper();
                        await _context.FinAccountBranches.AddAsync(acBranch);

                    }
                    await _context.SaveChangesAsync();


                    Log.Info("----Info SaveUpdateBranch method Exit----");

                    await transaction.CommitAsync();

                    return (string.Empty, Branch.Id);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in SaveUpdateBranch Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return (ApiMessageInfo.Failed, 0);
                }
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteBranch : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteBranchQueryHandler : IRequestHandler<DeleteBranch, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteBranchQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteBranch request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Branch);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in delete Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
