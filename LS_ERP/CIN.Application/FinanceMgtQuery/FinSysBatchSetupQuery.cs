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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{
    #region GetBatchSetupList

    public class GetBatchSetupList : IRequest<List<TblFinSysBatchSetupDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetBatchSetupListHandler : IRequestHandler<GetBatchSetupList, List<TblFinSysBatchSetupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBatchSetupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinSysBatchSetupDto>> Handle(GetBatchSetupList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBatchSetupList method start----");
            var item = await _context.BatchSetups.AsNoTracking()
                .OrderBy(e => e.Id)
               .ProjectTo<TblFinSysBatchSetupDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetBatchSetupList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSingleBatchSetup

    public class GetSingleBatchSetup : IRequest<TblFinSysBatchSetupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSingleBatchSetupHandler : IRequestHandler<GetSingleBatchSetup, TblFinSysBatchSetupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleBatchSetupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinSysBatchSetupDto> Handle(GetSingleBatchSetup request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSingleBatchSetup method start----");
            var item = await _context.BatchSetups.AsNoTracking()
                .Where(e => e.Id == request.Id)
               .ProjectTo<TblFinSysBatchSetupDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSingleBatchSetup method Ends----");
            return item;
        }
    }

    #endregion

    #region GetBatchSetupSelectList
    public class GetBatchSetupSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetBatchSetupSelectListHandler : IRequestHandler<GetBatchSetupSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBatchSetupSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBatchSetupSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.BatchSetups.AsNoTracking()
                .Where(e => e.IsActive)
              .Select(e => new CustomSelectListItem
              {
                  Text = isArab ? e.BatchName2 : e.BatchName,
                  Value = e.BatchCode,
              })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetBatchSetupSearchSelectList
    public class GetBatchSetupSearchSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetBatchSetupSearchSelectListHandler : IRequestHandler<GetBatchSetupSearchSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBatchSetupSearchSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBatchSetupSearchSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var search = request.Search;
            var list = await _context.BatchSetups.AsNoTracking()
                .Where(e => e.IsActive && (e.BatchName.Contains(search) || e.BatchName2.Contains(search)
                || e.BatchCode.Contains(search)))
              .Select(e => new CustomSelectListItem
              {
                  Text = isArab ? e.BatchName2 : e.BatchName,
                  Value = e.BatchCode,
              })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region CreateUpdateBatchSetup

    public class CreateUpdateBatchSetup : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinSysBatchSetupDto Input { get; set; }
    }

    public class CreateUpdateBatchSetupQueryHandler : IRequestHandler<CreateUpdateBatchSetup, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateBatchSetupQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateBatchSetup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateBatchSetup method start----");

                var obj = request.Input;
                TblFinSysBatchSetup seg = new();
                if (obj.Id > 0)
                    seg = await _context.BatchSetups.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else if (await _context.BatchSetups.AnyAsync(e => e.BatchCode == obj.BatchCode))
                    return ApiMessageInfo.DuplicateInfo(obj.BatchCode);

                seg.BatchName = obj.BatchName;
                seg.BatchName2 = obj.BatchName2;
                seg.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.BatchSetups.Update(seg);
                }
                else
                {
                    seg.BatchCode = obj.BatchCode.ToUpper();
                    await _context.BatchSetups.AddAsync(seg);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateBatchSetup method Exit----");
                return ApiMessageInfo.Status(1, seg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateBatchSetup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
}
