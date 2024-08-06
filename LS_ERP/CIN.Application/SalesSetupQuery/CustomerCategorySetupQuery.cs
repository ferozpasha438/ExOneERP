using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SalesSetupDtos;
using CIN.DB;
using CIN.Domain.SalesSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SalesSetupQuery
{

    #region GetCustCategoryList

    public class GetCustCategoryList : IRequest<List<TblSndDefCustomerCategoryDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCustCategoryListHandler : IRequestHandler<GetCustCategoryList, List<TblSndDefCustomerCategoryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustCategoryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSndDefCustomerCategoryDto>> Handle(GetCustCategoryList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCustCategoryList method start----");
            var list = await _context.SndCustomerCategories.AsNoTracking()
               .ProjectTo<TblSndDefCustomerCategoryDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetCustCategoryList method Ends----");
            return list;
        }
    }

    #endregion


    #region CreateUpdateCustCategory

    public class CreateUpdateCustCategory : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefCustomerCategoryDto Input { get; set; }
    }

    public class CreateUpdateCustCategoryHandler : IRequestHandler<CreateUpdateCustCategory, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateUpdateCustCategoryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(CreateUpdateCustCategory request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateCustCategory method start----");
                var obj = request.Input;

                if (await _context.SndCustomerCategories.AnyAsync(e => e.CatPrefix == obj.CatPrefix))
                    return ApiMessageInfo.Status(ApiMessageInfo.Duplicate("Category Prefix"));

                var custCategObj = await _context.SndCustomerCategories.FirstOrDefaultAsync(e => e.Id == obj.Id);

                TblSndDefCustomerCategory CustCateg = custCategObj ?? new();

                CustCateg.CustCatCode = obj.CustCatCode.ToUpper();
                CustCateg.CustCatName = obj.CustCatName;
                CustCateg.CatPrefix = obj.CatPrefix;
                CustCateg.CustCatDesc = obj.CustCatDesc;

                if (custCategObj is null)
                {
                    CustCateg.CreatedOn = DateTime.Now;
                    CustCateg.LastSeq = 0;
                    CustCateg.IsActive = true;
                    await _context.SndCustomerCategories.AddAsync(CustCateg);
                }
                else
                    _context.SndCustomerCategories.Update(CustCateg);

                await _context.SaveChangesAsync();
                return ApiMessageInfo.Status(1, CustCateg.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateCustCategory Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }

    #endregion
}
