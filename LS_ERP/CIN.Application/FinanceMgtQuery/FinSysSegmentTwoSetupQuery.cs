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
    #region GetSegmentTwoSetupList

    public class GetSegmentTwoSetupList : IRequest<List<TblFinSysSegmentTwoSetupDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSegmentTwoSetupListHandler : IRequestHandler<GetSegmentTwoSetupList, List<TblFinSysSegmentTwoSetupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSegmentTwoSetupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinSysSegmentTwoSetupDto>> Handle(GetSegmentTwoSetupList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSegmentTwoSetupList method start----");
            var item = await _context.SegmentTwoSetups.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .ProjectTo<TblFinSysSegmentTwoSetupDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSegmentTwoSetupList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSingleSegmentTwoSetup

    public class GetSingleSegmentTwoSetup : IRequest<TblFinSysSegmentTwoSetupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSingleSegmentTwoSetupHandler : IRequestHandler<GetSingleSegmentTwoSetup, TblFinSysSegmentTwoSetupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleSegmentTwoSetupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinSysSegmentTwoSetupDto> Handle(GetSingleSegmentTwoSetup request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSingleSegmentTwoSetup method start----");
            var item = await _context.SegmentTwoSetups.AsNoTracking()
                .Where(e => e.Id == request.Id)
               .ProjectTo<TblFinSysSegmentTwoSetupDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSingleSegmentTwoSetup method Ends----");
            return item;
        }
    }

    #endregion


    #region GetSegmentTwoSetupSelectList
    public class GetSegmentTwoSetupSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSegmentTwoSetupSelectListHandler : IRequestHandler<GetSegmentTwoSetupSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSegmentTwoSetupSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSegmentTwoSetupSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.SegmentTwoSetups.AsNoTracking()
                .Where(e => e.IsActive)
              .Select(e => new CustomSelectListItem
              {
                  Text = isArab ? e.Seg2Name2 : e.Seg2Name,
                  Value = e.Seg2Code,
              })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }
    #endregion

    #region GetSegmentTwoSetupSearchSelectList
    public class GetSegmentTwoSetupSearchSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetSegmentTwoSetupSearchSelectListHandler : IRequestHandler<GetSegmentTwoSetupSearchSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSegmentTwoSetupSearchSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSegmentTwoSetupSearchSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var search = request.Search;
            var list = await _context.SegmentTwoSetups.AsNoTracking()
                .Where(e => e.IsActive && (e.Seg2Name.Contains(search) || e.Seg2Name2.Contains(search)
                || e.Seg2Code.Contains(search)))
              .Select(e => new CustomSelectListItem
              {
                  Text = isArab ? e.Seg2Name2 : e.Seg2Name,
                  Value = e.Seg2Code,
              })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region CreateUpdateSegmentTwoSetup

    public class CreateUpdateSegmentTwoSetup : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinSysSegmentTwoSetupDto Input { get; set; }
    }

    public class CreateUpdateSegmentTwoSetupQueryHandler : IRequestHandler<CreateUpdateSegmentTwoSetup, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSegmentTwoSetupQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateSegmentTwoSetup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSegmentTwoSetup method start----");

                var obj = request.Input;
                TblFinSysSegmentTwoSetup seg = new();
                if (obj.Id > 0)
                    seg = await _context.SegmentTwoSetups.FirstOrDefaultAsync(e => e.Id == obj.Id);

                else if (await _context.SegmentTwoSetups.AnyAsync(e => e.Seg2Code == obj.Seg2Code))
                    return ApiMessageInfo.DuplicateInfo(obj.Seg2Code);

                seg.Seg2Name = obj.Seg2Name;
                seg.Seg2Name2 = obj.Seg2Name2;
                seg.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.SegmentTwoSetups.Update(seg);
                }
                else
                {
                    seg.Seg2Code = obj.Seg2Code.ToUpper();
                    await _context.SegmentTwoSetups.AddAsync(seg);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSegmentTwoSetup method Exit----");
                return ApiMessageInfo.Status(1, seg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSegmentTwoSetup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion
}
