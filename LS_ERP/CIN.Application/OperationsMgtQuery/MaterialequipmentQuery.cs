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

    #region GetMaterialequipmentsPagedList

    public class GetMaterialequipmentsPagedList : IRequest<PaginatedList<TblOpMaterialEquipmentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetMaterialequipmentsPagedListHandler : IRequestHandler<GetMaterialequipmentsPagedList, PaginatedList<TblOpMaterialEquipmentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaterialequipmentsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpMaterialEquipmentDto>> Handle(GetMaterialequipmentsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.TblOpMaterialEquipments.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.Code.Contains(search) ||
                            e.NameInArabic.Contains(search) ||
                            e.NameInEnglish.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblOpMaterialEquipmentDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateMaterialequipment
    public class CreateMaterialequipment : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpMaterialEquipmentDto MaterialequipmentDto { get; set; }
    }

    public class CreateMaterialequipmentHandler : IRequestHandler<CreateMaterialequipment, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateMaterialequipmentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateMaterialequipment request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateMaterialequipment method start----");



                var obj = request.MaterialequipmentDto;


                TblOpMaterialEquipment Materialequipment = new();
                if (obj.Id > 0)
                    Materialequipment = await _context.TblOpMaterialEquipments.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    var me = await _context.TblOpMaterialEquipments.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (me is not null)
                    {
                        Materialequipment.Code = "ME" + (me.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        Materialequipment.Code = "ME000001";

                    if (_context.TblOpMaterialEquipments.Any(x => x.Code == Materialequipment.Code))
                    {
                        return -1;
                    }
                }
                Materialequipment.Id = obj.Id;
                Materialequipment.IsActive = obj.IsActive;
                Materialequipment.NameInEnglish = obj.NameInEnglish;
                Materialequipment.NameInArabic = obj.NameInArabic;
                Materialequipment.Category = obj.Category;
                Materialequipment.Type = obj.Type;
                Materialequipment.EstimatedCostValue = obj.EstimatedCostValue;
                Materialequipment.IsDepreciationApplicable = obj.IsDepreciationApplicable;
                Materialequipment.MinimumCostPerUsage = obj.MinimumCostPerUsage;
                Materialequipment.DepreciationPerYear = obj.DepreciationPerYear;
                Materialequipment.UsageLifetermsYear = obj.UsageLifetermsYear;
                Materialequipment.Remarks = obj.Remarks;

                if (obj.Id > 0)
                {
                    Materialequipment.Code = obj.Code;
                    Materialequipment.ModifiedOn = DateTime.Now;
                    _context.TblOpMaterialEquipments.Update(Materialequipment);
                }
                else
                {

                    Materialequipment.CreatedOn = DateTime.Now;
                    await _context.TblOpMaterialEquipments.AddAsync(Materialequipment);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateMaterialequipment method Exit----");
                return Materialequipment.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateMaterialequipment Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetMaterialequipmentByCode
    public class GetMaterialequipmentByCode : IRequest<TblOpMaterialEquipmentDto>
    {
        public UserIdentityDto User { get; set; }
        public string MaterialequipmentCode { get; set; }
    }

    public class GetMaterialequipmentByMaterialequipmentCodeHandler : IRequestHandler<GetMaterialequipmentByCode, TblOpMaterialEquipmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaterialequipmentByMaterialequipmentCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpMaterialEquipmentDto> Handle(GetMaterialequipmentByCode request, CancellationToken cancellationToken)
        {
            TblOpMaterialEquipmentDto obj = await _context.TblOpMaterialEquipments.AsNoTracking().ProjectTo<TblOpMaterialEquipmentDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Code == request.MaterialequipmentCode);

            return obj;
        }
    }

    #endregion

    #region GetMaterialequipmentById
    public class GetMaterialequipmentById : IRequest<TblOpMaterialEquipmentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetMaterialequipmentByIdHandler : IRequestHandler<GetMaterialequipmentById, TblOpMaterialEquipmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaterialequipmentByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpMaterialEquipmentDto> Handle(GetMaterialequipmentById request, CancellationToken cancellationToken)
        {

            TblOpMaterialEquipmentDto obj = await _context.TblOpMaterialEquipments.AsNoTracking().ProjectTo<TblOpMaterialEquipmentDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return obj;
        }
    }

    #endregion

    #region GetSelectMaterialequipmentList

    public class GetSelectMaterialequipmentList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectMaterialequipmentListHandler : IRequestHandler<GetSelectMaterialequipmentList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectMaterialequipmentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectMaterialequipmentList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpMaterialEquipments.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.NameInEnglish, Value = e.Code, TextTwo = e.NameInArabic })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetAutoSelectMaterialEquipmentList

    public class GetAutoSelectMaterialEquipmentList : IRequest<List<TblOpMaterialEquipmentDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SearchKey;
    }

    public class GetAutoSelectMaterialEquipmentListHandler : IRequestHandler<GetAutoSelectMaterialEquipmentList, List<TblOpMaterialEquipmentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAutoSelectMaterialEquipmentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpMaterialEquipmentDto>> Handle(GetAutoSelectMaterialEquipmentList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpMaterialEquipments.AsNoTracking().ProjectTo<TblOpMaterialEquipmentDto>(_mapper.ConfigurationProvider).Where(e => e.Code.Contains(request.SearchKey) || e.NameInEnglish.Contains(request.SearchKey) ||
            e.NameInArabic.Contains(request.SearchKey)||request.SearchKey==null)
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetMaterialequipmentCodes

    public class GetMaterialequipmentCodes : IRequest<List<string>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetMaterialequipmentCodesHandler : IRequestHandler<GetMaterialequipmentCodes, List<string>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaterialequipmentCodesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<string>> Handle(GetMaterialequipmentCodes request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpMaterialEquipments.AsNoTracking()
              .Select(e => e.Code)
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteMaterialequipment
    public class DeleteMaterialequipment : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteMaterialequipmentQueryHandler : IRequestHandler<DeleteMaterialequipment, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteMaterialequipmentQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteMaterialequipment request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteMaterialequipment start----");

                if (request.Id > 0)
                {
                    var Materialequipment = await _context.TblOpMaterialEquipments.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Materialequipment);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteMaterialequipment");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
