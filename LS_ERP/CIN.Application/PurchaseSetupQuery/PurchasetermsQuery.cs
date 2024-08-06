using AutoMapper;
using CIN.Application.Common;
using CIN.Application.PurchaseSetupDtos;
using CIN.DB;
using CIN.Domain.PurchaseSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AutoMapper.QueryableExtensions;

namespace CIN.Application.PurchaseSetupQuery
{
    #region GetPagedList

    public class GetpotermList : IRequest<PaginatedList<TblPopDefVendorPOTermsCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetpotermListHandler : IRequestHandler<GetpotermList, PaginatedList<TblPopDefVendorPOTermsCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetpotermListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblPopDefVendorPOTermsCodeDto>> Handle(GetpotermList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.PopVendorPOTermsCodes.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.POTermsCode.Contains(search) || e.POTermsName.Contains(search) ||
                                e.POTermsDesc.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblPopDefVendorPOTermsCodeDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region CreateUpdate

    public class CreatePurchaseterms : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPopDefVendorPOTermsCodeDto Input { get; set; }
    }

    public class CreatePurchasetermsQueryHandler : IRequestHandler<CreatePurchaseterms, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePurchasetermsQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreatePurchaseterms request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreatePurchaseterms method start----");

                var obj = request.Input;
                TblPopDefVendorPOTermsCode cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.POTermsCode = obj.POTermsCode;
                cObj.POTermsName = obj.POTermsName;
                cObj.POTermsDesc = obj.POTermsDesc; 
                cObj.POTermsDueDays = obj.POTermsDueDays;
                cObj.POTermDiscDays = obj.POTermDiscDays;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.PopVendorPOTermsCodes.Update(cObj);
                }
                else
                {
                    cObj.POTermsCode = obj.POTermsCode.ToUpper();
                    cObj.CreatedOn = DateTime.Now;

                    await _context.PopVendorPOTermsCodes.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreatePurchaseterms method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreatePurchaseterms Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
    #region Delete
    public class DeletePoterm : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletepotermQueryHandler : IRequestHandler<DeletePoterm, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletepotermQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePoterm request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Category = await _context.PopVendorPOTermsCodes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Category);
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
    #region SingleItem

    public class GetPoterm : IRequest<TblPopDefVendorPOTermsCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPotermHandler : IRequestHandler<GetPoterm, TblPopDefVendorPOTermsCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPotermHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorPOTermsCodeDto> Handle(GetPoterm request, CancellationToken cancellationToken)
        {
            var item = await _context.PopVendorPOTermsCodes.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblPopDefVendorPOTermsCodeDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
    #region GetPotermcode

    public class GetPurchaseTerm : IRequest<TblPopDefVendorPOTermsCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public string PotermCode { get; set; }
    }

    public class GetPurchasetermHandler : IRequestHandler<GetPurchaseTerm, TblPopDefVendorPOTermsCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPurchasetermHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblPopDefVendorPOTermsCodeDto> Handle(GetPurchaseTerm request, CancellationToken cancellationToken)
        {
            var item = await _context.PopVendorPOTermsCodes.AsNoTracking()
                   .Where(e => e.POTermsCode == request.PotermCode)
              .ProjectTo<TblPopDefVendorPOTermsCodeDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
