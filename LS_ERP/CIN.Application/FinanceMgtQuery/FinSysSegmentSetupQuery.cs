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


    #region GetSegmentSetupList

    public class GetSegmentSetupList : IRequest<List<TblFinSysSegmentSetupDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSegmentSetupListHandler : IRequestHandler<GetSegmentSetupList, List<TblFinSysSegmentSetupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSegmentSetupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinSysSegmentSetupDto>> Handle(GetSegmentSetupList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSegmentSetupList method start----");
            var item = await _context.SegmentSetups.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .ProjectTo<TblFinSysSegmentSetupDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSegmentSetupList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetSingleSegmentSetup

    public class GetSingleSegmentSetup : IRequest<TblFinSysSegmentSetupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSingleSegmentSetupHandler : IRequestHandler<GetSingleSegmentSetup, TblFinSysSegmentSetupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSingleSegmentSetupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinSysSegmentSetupDto> Handle(GetSingleSegmentSetup request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSingleSegmentSetup method start----");
            var item = await _context.SegmentSetups.AsNoTracking()
                .Where(e => e.Id == request.Id)
               .ProjectTo<TblFinSysSegmentSetupDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSingleSegmentSetup method Ends----");
            return item;
        }
    }

    #endregion


    #region GetSegmentSetupSearchSelectList
    public class GetSegmentSetupSearchSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetSegmentSetupSearchSelectListHandler : IRequestHandler<GetSegmentSetupSearchSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSegmentSetupSearchSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSegmentSetupSearchSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var search = request.Search;
            var list = await _context.SegmentSetups.AsNoTracking()
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

    #region GetSegmentSetupSelectList
    public class GetSegmentSetupSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSegmentSetupSelectListHandler : IRequestHandler<GetSegmentSetupSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSegmentSetupSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSegmentSetupSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.SegmentSetups.AsNoTracking()
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


    #region CreateUpdateSegmentSetup

    public class CreateUpdateSegmentSetup : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinSysSegmentSetupDto Input { get; set; }
    }

    public class CreateUpdateSegmentSetupQueryHandler : IRequestHandler<CreateUpdateSegmentSetup, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSegmentSetupQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateSegmentSetup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSegmentSetup method start----");

                var obj = request.Input;
                TblFinSysSegmentSetup seg = new();
                if (obj.Id > 0)
                    seg = await _context.SegmentSetups.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else if (await _context.SegmentSetups.AnyAsync(e => e.Seg2Code == obj.Seg2Code))
                    return ApiMessageInfo.DuplicateInfo(obj.Seg2Code);

                seg.Seg2Name = obj.Seg2Name;
                seg.Seg2Name2 = obj.Seg2Name2;
                seg.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.SegmentSetups.Update(seg);
                }
                else
                {
                    seg.Seg2Code = obj.Seg2Code.ToUpper();
                    await _context.SegmentSetups.AddAsync(seg);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSegmentSetup method Exit----");
                return ApiMessageInfo.Status(1, seg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSegmentSetup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


}
