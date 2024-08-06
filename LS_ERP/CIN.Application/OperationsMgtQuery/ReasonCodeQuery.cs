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

    #region GetReasonCodesPagedList

    public class GetReasonCodesPagedList : IRequest<PaginatedList<TblOpReasonCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetReasonCodesPagedListHandler : IRequestHandler<GetReasonCodesPagedList, PaginatedList<TblOpReasonCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReasonCodesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpReasonCodeDto>> Handle(GetReasonCodesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprReasonCodes.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ReasonCode.Contains(search) ||
                            e.ReasonCodeNameEng.Contains(search) ||
                            e.ReasonCodeNameArb.Contains(search) ||
                            e.DescriptionArb.Contains(search) ||
                            e.DescriptionEng.Contains(search) ||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblOpReasonCodeDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateReasonCode
    public class CreateReasonCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpReasonCodeDto ReasonCodeDto { get; set; }
    }

    public class CreateReasonCodeHandler : IRequestHandler<CreateReasonCode,int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateReasonCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateReasonCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateReasonCode method start----");



                var obj = request.ReasonCodeDto;


                TblOpReasonCode ReasonCode = new();
                if (obj.Id > 0)
                    ReasonCode = await _context.OprReasonCodes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {

                    var sur = await _context.OprReasonCodes.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (sur is not null)
                    {
                        ReasonCode.ReasonCode = "REAS" + (sur.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        ReasonCode.ReasonCode = "REAS000001";

                    if (_context.OprReasonCodes.Any(x => x.ReasonCode == ReasonCode.ReasonCode))
                    {
                        return -1;
                    }                   
                    
                    //ReasonCode.ReasonCode = obj.ReasonCode.ToUpper();
                }
               // ReasonCode.Id = obj.Id;
               ReasonCode.IsActive = obj.IsActive;
                ReasonCode.ReasonCodeNameEng = obj.ReasonCodeNameEng;
                ReasonCode.ReasonCodeNameArb = obj.ReasonCodeNameArb;
                ReasonCode.DescriptionEng = obj.DescriptionEng;
                ReasonCode.DescriptionArb = obj.DescriptionArb;
                ReasonCode.IsForCustomerComplaint = obj.IsForCustomerComplaint;
                ReasonCode.IsForCustomerVisit = obj.IsForCustomerVisit;

                if (obj.Id > 0)
                {
                    ReasonCode.ReasonCode = obj.ReasonCode;
                   ReasonCode.ModifiedOn = DateTime.Now;
                   ReasonCode.ModifiedBy = request.User.UserId;

                    _context.OprReasonCodes.Update(ReasonCode);
                    await _context.SaveChangesAsync();

                }
                else
                {

                    ReasonCode.CreatedOn = DateTime.Now;
                    ReasonCode.CreatedBy = request.User.UserId;

                    await _context.OprReasonCodes.AddAsync(ReasonCode);
                    await _context.SaveChangesAsync();

                }
                Log.Info("----Info CreateUpdateReasonCode method Exit----");
                return ReasonCode.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateReasonCode Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetReasonCodeByReasonCode
    public class GetReasonCodeByReasonCode : IRequest<TblOpReasonCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public string ReasonCode { get; set; }
    }

    public class GetReasonCodeByReasonCodeHandler : IRequestHandler<GetReasonCodeByReasonCode, TblOpReasonCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReasonCodeByReasonCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpReasonCodeDto> Handle(GetReasonCodeByReasonCode request, CancellationToken cancellationToken)
        {
            TblOpReasonCodeDto obj = new();
            var ReasonCode = await _context.OprReasonCodes.AsNoTracking().ProjectTo<TblOpReasonCodeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.ReasonCode == request.ReasonCode);
            return ReasonCode;
        }

    }

    #endregion

    #region GetReasonCodeById
    public class GetReasonCodeById : IRequest<TblOpReasonCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetReasonCodeByIdHandler : IRequestHandler<GetReasonCodeById, TblOpReasonCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReasonCodeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpReasonCodeDto> Handle(GetReasonCodeById request, CancellationToken cancellationToken)
        {
            TblOpReasonCodeDto obj = new();
            var ReasonCode = await _context.OprReasonCodes.AsNoTracking().ProjectTo<TblOpReasonCodeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
          
            return ReasonCode;
        }
    }

    #endregion

    #region GetSelectReasonCodeList

    public class GetSelectReasonCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectReasonCodeListHandler : IRequestHandler<GetSelectReasonCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectReasonCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectReasonCodeList request, CancellationToken cancellationToken)
        {

            var list = await _context.OprReasonCodes.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.ReasonCodeNameEng, Value = e.ReasonCode, TextTwo = e.ReasonCodeNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion



    #region GetSelectReasonCodeListForCustomerVisit

    public class GetSelectReasonCodeListForCustomerVisit : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectReasonCodeListForCustomerVisitHandler : IRequestHandler<GetSelectReasonCodeListForCustomerVisit, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectReasonCodeListForCustomerVisitHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectReasonCodeListForCustomerVisit request, CancellationToken cancellationToken)
        {

            var list = await _context.OprReasonCodes.Where(e=>e.IsForCustomerVisit.Value==true)
              .Select(e => new CustomSelectListItem { Text = e.ReasonCodeNameEng, Value = e.ReasonCode, TextTwo = e.ReasonCodeNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetSelectReasonCodeListForCustomerComplaint

    public class GetSelectReasonCodeListForCustomerComplaint : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectReasonCodeListForCustomerComplaintHandler : IRequestHandler<GetSelectReasonCodeListForCustomerComplaint, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectReasonCodeListForCustomerComplaintHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectReasonCodeListForCustomerComplaint request, CancellationToken cancellationToken)
        {

            var list = await _context.OprReasonCodes.Where(e => e.IsForCustomerComplaint.Value == true)
              .Select(e => new CustomSelectListItem { Text = e.ReasonCodeNameEng, Value = e.ReasonCode, TextTwo = e.ReasonCodeNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion












    #region DeleteReasonCode
    public class DeleteReasonCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteReasonCodeQueryHandler : IRequestHandler<DeleteReasonCode, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteReasonCodeQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteReasonCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteReasonCode start----");

                if (request.Id > 0)
                {
                    var ReasonCode = await _context.OprReasonCodes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(ReasonCode);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteReasonCode");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
