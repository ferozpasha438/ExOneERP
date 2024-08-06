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

    #region GetCustomerSurveyFormElementsPagedList

    public class GetSurveyFormElementsPagedList : IRequest<PaginatedList<TblSndDefSurveyFormElementDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSurveyFormElementsPagedListHandler : IRequestHandler<GetSurveyFormElementsPagedList, PaginatedList<TblSndDefSurveyFormElementDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormElementsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefSurveyFormElementDto>> Handle(GetSurveyFormElementsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprSurveyFormElements.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ElementArbName.Contains(search) ||
                            e.ElementEngName.Contains(search) ||
                            e.FormElementCode.Contains(search) ||
                            e.ElementType.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefSurveyFormElementDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateSurveyFormElement
    public class CreateSurveyFormElement : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefSurveyFormElementDto SurveyFormElementDto { get; set; }
    }

    public class CreateSurveyFormElementHandler : IRequestHandler<CreateSurveyFormElement, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSurveyFormElementHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSurveyFormElement request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSurveyFormElement method start----");



                var obj = request.SurveyFormElementDto;


                TblSndDefSurveyFormElement SurveyFormElement = new();
                if (obj.Id > 0)


                    SurveyFormElement = await _context.OprSurveyFormElements.FirstOrDefaultAsync(e => e.Id == obj.Id);





                else
                {

                    var sfe = await _context.OprSurveyFormElements.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (sfe is not null)
                    {
                        SurveyFormElement.FormElementCode = "SFE" + (sfe.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        SurveyFormElement.FormElementCode = "SFE000001";

                    if (_context.OprSurveyFormElements.Any(x => x.FormElementCode == SurveyFormElement.FormElementCode))
                    {
                        return -1;
                    }


                }









                    //if (_context.OprSurveyFormElements.Any(x => x.FormElementCode == obj.FormElementCode))
                    //{
                    //    return -1;
                    //}
                    //  SurveyFormElement.FormElementCode = obj.FormElementCode.ToUpper();
                
                //SurveyFormElement.Id = obj.Id;
                //SurveyFormElement.ModifiedOn = obj.ModifiedOn;
                SurveyFormElement.IsActive = obj.IsActive;
               
                SurveyFormElement.ElementEngName = obj.ElementEngName;
                SurveyFormElement.ElementArbName = obj.ElementArbName;
                SurveyFormElement.ElementType = obj.ElementType;
                SurveyFormElement.ListValueString = obj.ListValueString;
                SurveyFormElement.MinValue = obj.MinValue;
                SurveyFormElement.MaxValue = obj.MaxValue;


                if (obj.Id > 0)
                {
                    SurveyFormElement.ModifiedOn = DateTime.Now;

                    _context.OprSurveyFormElements.Update(SurveyFormElement);
                }
                else
                {
                    SurveyFormElement.CreatedOn = DateTime.Now;
                    await _context.OprSurveyFormElements.AddAsync(SurveyFormElement);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSurveyFormElement method Exit----");
                return SurveyFormElement.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSurveyFormElement Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetSurveyFormElementByElementCode
    public class GetSurveyFormElementByElementCode : IRequest<TblSndDefSurveyFormElementDto>
    {
        public UserIdentityDto User { get; set; }
        public string SurveyFormElementCode { get; set; }
    }

    public class GetSurveyFormElementByElementCodeHandler : IRequestHandler<GetSurveyFormElementByElementCode, TblSndDefSurveyFormElementDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormElementByElementCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyFormElementDto> Handle(GetSurveyFormElementByElementCode request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyFormElementDto obj = new();
            var SurveyFormElement = await _context.OprSurveyFormElements.AsNoTracking().FirstOrDefaultAsync(e => e.FormElementCode == request.SurveyFormElementCode);
            if (SurveyFormElement is not null)
            {
                obj.Id = SurveyFormElement.Id;
                obj.FormElementCode = SurveyFormElement.FormElementCode;
                obj.ElementEngName = SurveyFormElement.ElementEngName;
                obj.ElementArbName = SurveyFormElement.ElementArbName;
                obj.ElementType = SurveyFormElement.ElementType;
                obj.ListValueString = SurveyFormElement.ListValueString;
                obj.MinValue = SurveyFormElement.MinValue;
                obj.MaxValue = SurveyFormElement.MaxValue;

                obj.ModifiedOn = SurveyFormElement.ModifiedOn;
                obj.CreatedOn = SurveyFormElement.CreatedOn;
                obj.IsActive = SurveyFormElement.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetSurveyFormElementById
    public class GetSurveyFormElementById : IRequest<TblSndDefSurveyFormElementDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSurveyFormElementByIdHandler : IRequestHandler<GetSurveyFormElementById, TblSndDefSurveyFormElementDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSurveyFormElementByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefSurveyFormElementDto> Handle(GetSurveyFormElementById request, CancellationToken cancellationToken)
        {
            TblSndDefSurveyFormElementDto obj = new();
            var SurveyFormElement = await _context.OprSurveyFormElements.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (SurveyFormElement is not null)
            {
                obj.Id = SurveyFormElement.Id;
                obj.FormElementCode = SurveyFormElement.FormElementCode;
                obj.ElementEngName = SurveyFormElement.ElementEngName;
                obj.ElementArbName = SurveyFormElement.ElementArbName;
                obj.ElementType = SurveyFormElement.ElementType;
                obj.ListValueString = SurveyFormElement.ListValueString;
                obj.MinValue = SurveyFormElement.MinValue;
                obj.MaxValue = SurveyFormElement.MaxValue;
                obj.ModifiedOn = SurveyFormElement.ModifiedOn;
                obj.CreatedOn = SurveyFormElement.CreatedOn;
                obj.IsActive = SurveyFormElement.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetSelectSurveyFormElementList

    public class GetSelectSurveyFormElementList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
       
    }

    public class GetSelectSurveyFormElementListHandler : IRequestHandler<GetSelectSurveyFormElementList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSurveyFormElementListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSurveyFormElementList request, CancellationToken cancellationToken)
        {
            
            var list = await _context.OprSurveyFormElements.AsNoTracking()
                
              .Select(e => new CustomSelectListItem { Text = e.ElementEngName, TextTwo = e.ElementArbName, Value = e.FormElementCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteSurveyFormElement
    public class DeleteSurveyFormElement : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSurveyFormElementQueryHandler : IRequestHandler<DeleteSurveyFormElement, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSurveyFormElementQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSurveyFormElement request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSurveyFormElement start----");

                if (request.Id > 0)
                {
                    var formElement = await _context.OprSurveyFormElements.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(formElement);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSurveyFormElement");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
