using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SalesSetupDtos;
using CIN.DB;
using CIN.Domain.SalesSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SalesSetupQuery
{
    #region GetPagedList

    public class GetSalesTermList : IRequest<PaginatedList<TblSndDefSalesTermsCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSalesTermListHandler : IRequestHandler<GetSalesTermList, PaginatedList<TblSndDefSalesTermsCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSalesTermListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefSalesTermsCodeDto>> Handle(GetSalesTermList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.SndSalesTermsCodes.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.SalesTermsCode.Contains(search) || e.SalesTermsName.Contains(search) ||
                                e.SalesTermsDesc.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefSalesTermsCodeDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion
    #region CreateUpdate

    public class CreateSalesTerm : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefSalesTermsCodeDto Input { get; set; }
    }

    public class CreateSalesTermQueryHandler : IRequestHandler<CreateSalesTerm, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSalesTermQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateSalesTerm request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateSalesTerm method start----");

                var obj = request.Input;
                TblSndDefSalesTermsCode cObj = new();
                if (obj.Id > 0)
                    cObj = await _context.SndSalesTermsCodes.FirstOrDefaultAsync(e => e.Id == obj.Id);

                cObj.SalesTermsCode = obj.SalesTermsCode;
                cObj.SalesTermsName = obj.SalesTermsName;
                cObj.SalesTermsDesc = obj.SalesTermsDesc;
                cObj.SalesTermsDueDays = obj.SalesTermsDueDays;
                cObj.SalesTermDiscDays = obj.SalesTermDiscDays;

                if (obj.Id > 0)
                {
                    cObj.ModifiedOn = DateTime.Now;
                    _context.SndSalesTermsCodes.Update(cObj);
                }
                else
                {
                    cObj.SalesTermsCode = obj.SalesTermsCode.ToUpper();
                    cObj.CreatedOn = DateTime.Now;

                    await _context.SndSalesTermsCodes.AddAsync(cObj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateSalesTerm method Exit----");
                return ApiMessageInfo.Status(1, cObj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateSalesTerm Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
    #region Delete
    public class DeleteSalesTerm : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSalesTermQueryHandler : IRequestHandler<DeleteSalesTerm, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSalesTermQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSalesTerm request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Category = await _context.SndSalesTermsCodes.FirstOrDefaultAsync(e => e.Id == request.Id);
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

    public class GetSalesTerm : IRequest<TblSndDefSalesTermsCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSalesTermHandler : IRequestHandler<GetSalesTerm, TblSndDefSalesTermsCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSalesTermHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSalesTermsCodeDto> Handle(GetSalesTerm request, CancellationToken cancellationToken)
        {
            var item = await _context.SndSalesTermsCodes.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblSndDefSalesTermsCodeDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
    #region GetSalesTermcode

    public class GetSalesTermByCode : IRequest<TblSndDefSalesTermsCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public string PotermCode { get; set; }
    }

    public class GetSalesTermByCodeHandler : IRequestHandler<GetSalesTermByCode, TblSndDefSalesTermsCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSalesTermByCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSalesTermsCodeDto> Handle(GetSalesTermByCode request, CancellationToken cancellationToken)
        {
            var item = await _context.SndSalesTermsCodes.AsNoTracking()
                   .Where(e => e.SalesTermsCode == request.PotermCode)
              .ProjectTo<TblSndDefSalesTermsCodeDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
