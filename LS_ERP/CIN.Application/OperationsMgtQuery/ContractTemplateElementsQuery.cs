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
using CIN.Domain.SystemSetup;

namespace CIN.Application.OperationsMgtQuery
{


    #region CreateUpdateContractFormTemplateElement
    public class CreateContractFormTemplateElement : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpContractClauseDto Input { get; set; }
    }

    public class CreateContractFormTemplateElementHandler : IRequestHandler<CreateContractFormTemplateElement, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateContractFormTemplateElementHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateContractFormTemplateElement request, CancellationToken cancellationToken)
        {
           
                try
                {
                    Log.Info("----Info CreateUpdateContractFormTemplateElement method start----");
                    TblOpContractClause obj = new();
                    if (request.Input.Id>0) { 
                           obj = await _context.TblOpContractClauses.AsNoTracking().FirstOrDefaultAsync(e=>e.Id==request.Input.Id);
                    }


                    obj.ClauseTitleEng = request.Input.ClauseTitleEng;
                    obj.ClauseTitleArb = request.Input.ClauseTitleArb;
                    obj.ClauseSubTitleEng = request.Input.ClauseSubTitleEng;
                    obj.ClauseSubTitleArb = request.Input.ClauseSubTitleArb;
                    obj.ClauseDescriptionEng = request.Input.ClauseDescriptionEng;
                    obj.ClauseDescriptionArb = request.Input.ClauseDescriptionArb;
                    //obj.NumberArb = request.Input.NumberArb;
                    //obj.NumberEng = request.Input.NumberEng;
                    if (request.Input.Id > 0)
                    {
                        _context.TblOpContractClauses.Update(obj);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        await _context.TblOpContractClauses.AddAsync(obj);
                        await _context.SaveChangesAsync();

                    }


                    return obj.Id;


                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateContractFormTemplateElement Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        
    }

    #endregion


    #region GetContractFormTemplateElementById
    public class GetContractFormTemplateElementById : IRequest<TblOpContractClauseDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetContractFormTemplateElementByIdHandler : IRequestHandler<GetContractFormTemplateElementById, TblOpContractClauseDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormTemplateElementByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpContractClauseDto> Handle(GetContractFormTemplateElementById request, CancellationToken cancellationToken)
        {
            var contractClause = await _context.TblOpContractClauses.ProjectTo<TblOpContractClauseDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e=>e.Id==request.Id);

            return contractClause;

        }
    }

    #endregion






    #region GetContractFormTemplateElementsSelectionList
    public class GetContractFormTemplateElementsSelectionList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
      
    }

    public class GetContractFormTemplateElementsSelectionListHandler : IRequestHandler<GetContractFormTemplateElementsSelectionList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormTemplateElementsSelectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetContractFormTemplateElementsSelectionList request, CancellationToken cancellationToken)
        {
            
           var TemplateClauses = await _context.TblOpContractClauses.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ClauseTitleEng, Value = e.Id.ToString(),TextTwo=e.ClauseTitleArb })
                  .ToListAsync(cancellationToken);
            return TemplateClauses;

        }
    }

    #endregion



    #region GetContractFormTemplateElementsPagedList

    public class GetContractFormTemplateElementsPagedList : IRequest<PaginatedList<ContractFormTemplateElementsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetContractFormTemplateElementsPagedListHandler : IRequestHandler<GetContractFormTemplateElementsPagedList, PaginatedList<ContractFormTemplateElementsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractFormTemplateElementsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ContractFormTemplateElementsPaginationDto>> Handle(GetContractFormTemplateElementsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.TblOpContractClauses.ProjectTo<ContractFormTemplateElementsPaginationDto>(_mapper.ConfigurationProvider)
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ClauseTitleEng.Contains(search) ||
                            e.ClauseTitleArb.Contains(search) ||
                            e.ClauseSubTitleEng.Contains(search) || e.ClauseSubTitleArb.Contains(search) ||
                            e.ClauseDescriptionEng.Contains(search) || e.ClauseDescriptionArb.Contains(search) ||
                            
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)              
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion


    #region GetAllContractFormTemplateElementsSelectionList
    public class GetAllContractFormTemplateElementsSelectionList : IRequest<List<TblOpContractClauseDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetAllContractFormTemplateElementsSelectionListHandler : IRequestHandler<GetAllContractFormTemplateElementsSelectionList, List<TblOpContractClauseDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllContractFormTemplateElementsSelectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpContractClauseDto>> Handle(GetAllContractFormTemplateElementsSelectionList request, CancellationToken cancellationToken)
        {

            var TemplateClauses =  await _context.TblOpContractClauses.AsNoTracking().ProjectTo< TblOpContractClauseDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Id)
                
                   .ToListAsync(cancellationToken);
            return TemplateClauses;

        }
    }

    #endregion

}
