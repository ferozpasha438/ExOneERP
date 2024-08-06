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

    #region GetOperationExpenseHeadsPagedList

    public class GetOperationExpenseHeadsPagedList : IRequest<PaginatedList<TblOpOperationExpenseHeadDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetOperationExpenseHeadsPagedListHandler : IRequestHandler<GetOperationExpenseHeadsPagedList, PaginatedList<TblOpOperationExpenseHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpOperationExpenseHeadDto>> Handle(GetOperationExpenseHeadsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.TblOpOperationExpenseHeads.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.CostHead.Contains(search) ||
                            e.CostNameInEnglish.Contains(search) ||
                            e.CostNameInArabic.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateOperationExpenseHead
    public class CreateOperationExpenseHead : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpOperationExpenseHeadDto OperationExpenseHeadDto { get; set; }
    }

    public class CreateOperationExpenseHeadHandler : IRequestHandler<CreateOperationExpenseHead, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOperationExpenseHeadHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOperationExpenseHead request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateOperationExpenseHead method start----");



                var obj = request.OperationExpenseHeadDto;


                TblOpOperationExpenseHead OperationExpenseHead = new();
                if (obj.Id > 0)
                    OperationExpenseHead = await _context.TblOpOperationExpenseHeads.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {


                    var ch = await _context.TblOpOperationExpenseHeads.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (ch is not null)
                    {
                        OperationExpenseHead.CostHead = "CH" + (ch.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        OperationExpenseHead.CostHead = "CH000001";

                    if (_context.TblOpOperationExpenseHeads.Any(x => x.CostHead == OperationExpenseHead.CostHead))
                    {
                        return -1;
                    }
                }
                OperationExpenseHead.CostType = obj.CostType;
                OperationExpenseHead.CostNameInEnglish = obj.CostNameInEnglish;
                OperationExpenseHead.CostNameInArabic = obj.CostNameInArabic;
                OperationExpenseHead.MinServiceCosttoCompany = obj.MinServiceCosttoCompany;
                OperationExpenseHead.MinServicePrice = obj.MinServicePrice;
                OperationExpenseHead.GrossMargin = obj.GrossMargin;
                OperationExpenseHead.IsActive = obj.IsActive;
                OperationExpenseHead.isApplicableForVehicle = obj.isApplicableForVehicle;
                OperationExpenseHead.isApplicableForSkillset = obj.isApplicableForSkillset;
                OperationExpenseHead.isApplicableForMaterial = obj.isApplicableForMaterial;
                OperationExpenseHead.isApplicableForFinancialExpense = obj.isApplicableForFinancialExpense;
                OperationExpenseHead.Remarks = obj.Remarks;



                if (obj.Id > 0)
                {
                    OperationExpenseHead.CostHead = obj.CostHead;
                    OperationExpenseHead.ModifiedOn = DateTime.Now;
                    _context.TblOpOperationExpenseHeads.Update(OperationExpenseHead);
                }
                else
                {

                    OperationExpenseHead.CreatedOn = DateTime.Now;
                    await _context.TblOpOperationExpenseHeads.AddAsync(OperationExpenseHead);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateOperationExpenseHead method Exit----");
                return OperationExpenseHead.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateOperationExpenseHead Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetOperationExpenseHeadByCode
    public class GetOperationExpenseHeadByCode : IRequest<TblOpOperationExpenseHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public string OperationExpenseHeadCode { get; set; }
    }

    public class GetOperationExpenseHeadByOperationExpenseHeadCodeHandler : IRequestHandler<GetOperationExpenseHeadByCode, TblOpOperationExpenseHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadByOperationExpenseHeadCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpOperationExpenseHeadDto> Handle(GetOperationExpenseHeadByCode request, CancellationToken cancellationToken)
        {
            TblOpOperationExpenseHeadDto obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.CostHead == request.OperationExpenseHeadCode);

            return obj;
        }
    }

    #endregion

    #region GetOperationExpenseHeadById
    public class GetOperationExpenseHeadById : IRequest<TblOpOperationExpenseHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetOperationExpenseHeadByIdHandler : IRequestHandler<GetOperationExpenseHeadById, TblOpOperationExpenseHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpOperationExpenseHeadDto> Handle(GetOperationExpenseHeadById request, CancellationToken cancellationToken)
        {

            TblOpOperationExpenseHeadDto obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return obj;
        }
    }

    #endregion

    #region GetSelectOperationExpenseHeadList

    public class GetSelectOperationExpenseHeadList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectOperationExpenseHeadListHandler : IRequestHandler<GetSelectOperationExpenseHeadList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectOperationExpenseHeadListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectOperationExpenseHeadList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpOperationExpenseHeads.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.CostNameInEnglish, Value = e.CostHead, TextTwo = e.CostNameInArabic })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetOperationExpenseHeadCodes

    public class GetOperationExpenseHeadCodes : IRequest<List<string>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetOperationExpenseHeadCodesHandler : IRequestHandler<GetOperationExpenseHeadCodes, List<string>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadCodesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<string>> Handle(GetOperationExpenseHeadCodes request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpOperationExpenseHeads.AsNoTracking()
              .Select(e => e.CostHead)
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteOperationExpenseHead
    public class DeleteOperationExpenseHead : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteOperationExpenseHeadQueryHandler : IRequestHandler<DeleteOperationExpenseHead, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteOperationExpenseHeadQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteOperationExpenseHead request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteOperationExpenseHead start----");

                if (request.Id > 0)
                {
                    var OperationExpenseHead = await _context.TblOpOperationExpenseHeads.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(OperationExpenseHead);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteOperationExpenseHead");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion


    #region GetOperationExpenseHeadsForResources
    public class GetOperationExpenseHeadsForResources : IRequest<List<TblOpOperationExpenseHeadDto>>
    {
        public UserIdentityDto User { get; set; }
        
    }

    public class GetOperationExpenseHeadsForResourcesHandler : IRequestHandler<GetOperationExpenseHeadsForResources, List<TblOpOperationExpenseHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadsForResourcesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpOperationExpenseHeadDto>> Handle(GetOperationExpenseHeadsForResources request, CancellationToken cancellationToken)
        {
            List<TblOpOperationExpenseHeadDto> obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).Where(e => e.isApplicableForSkillset==true).ToListAsync();

            return obj;
        }
    }

    #endregion

    #region GetOperationExpenseHeadsForLogistics
    public class GetOperationExpenseHeadsForLogistics : IRequest<List<TblOpOperationExpenseHeadDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetOperationExpenseHeadsForLogisticsHandler : IRequestHandler<GetOperationExpenseHeadsForLogistics, List<TblOpOperationExpenseHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadsForLogisticsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpOperationExpenseHeadDto>> Handle(GetOperationExpenseHeadsForLogistics request, CancellationToken cancellationToken)
        {
            List<TblOpOperationExpenseHeadDto> obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).Where(e => e.isApplicableForVehicle == true).ToListAsync();

            return obj;
        }
    }

    #endregion

#region GetOperationExpenseHeadsForMaterialEquipment
    public class GetOperationExpenseHeadsForMaterialEquipment : IRequest<List<TblOpOperationExpenseHeadDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetOperationExpenseHeadsForMaterialEquipmentHandler : IRequestHandler<GetOperationExpenseHeadsForMaterialEquipment, List<TblOpOperationExpenseHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadsForMaterialEquipmentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpOperationExpenseHeadDto>> Handle(GetOperationExpenseHeadsForMaterialEquipment request, CancellationToken cancellationToken)
        {
            List<TblOpOperationExpenseHeadDto> obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).Where(e => e.isApplicableForMaterial == true).ToListAsync();

            return obj;
        }
    }

    #endregion
    
    #region GetOperationExpenseHeadsForFinancialExpence
    public class GetOperationExpenseHeadsForFinancialExpence : IRequest<List<TblOpOperationExpenseHeadDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetOperationExpenseHeadsForFinancialExpenceHandler : IRequestHandler<GetOperationExpenseHeadsForFinancialExpence, List<TblOpOperationExpenseHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOperationExpenseHeadsForFinancialExpenceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpOperationExpenseHeadDto>> Handle(GetOperationExpenseHeadsForFinancialExpence request, CancellationToken cancellationToken)
        {
            List<TblOpOperationExpenseHeadDto> obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).Where(e => e.isApplicableForFinancialExpense == true).ToListAsync();

            return obj;
        }
    }

    #endregion

    #region GetAutoSelectListForFinancialExpense
    public class GetAutoSelectListForFinancialExpense : IRequest<List<TblOpOperationExpenseHeadDto>>
    {
        public UserIdentityDto User { get; set; }
        public string search { get; set; }
    }

    public class GetAutoSelectListForFinancialExpenseHandler : IRequestHandler<GetAutoSelectListForFinancialExpense, List<TblOpOperationExpenseHeadDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAutoSelectListForFinancialExpenseHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpOperationExpenseHeadDto>> Handle(GetAutoSelectListForFinancialExpense request, CancellationToken cancellationToken)
        {
            List<TblOpOperationExpenseHeadDto> obj = await _context.TblOpOperationExpenseHeads.AsNoTracking().ProjectTo<TblOpOperationExpenseHeadDto>(_mapper.ConfigurationProvider).Where(e => e.isApplicableForFinancialExpense == true && 
            (e.CostHead.Contains(request.search)||e.CostNameInArabic.Contains(request.search)||e.CostNameInEnglish.Contains(request.search)||request.search==null)).ToListAsync();

            return obj;
        }
    }

    #endregion
}
