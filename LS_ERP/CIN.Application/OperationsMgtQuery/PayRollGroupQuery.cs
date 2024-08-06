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

    #region GetPayRollGroupsPagedList

    public class GetPayRollGroupsPagedList : IRequest<PaginatedList<HRM_DEF_PayrollGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPayRollGroupsPagedListHandler : IRequestHandler<GetPayRollGroupsPagedList, PaginatedList<HRM_DEF_PayrollGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayRollGroupsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<HRM_DEF_PayrollGroupDto>> Handle(GetPayRollGroupsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.HRM_DEF_PayrollGroups.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.PayrollGroupID.ToString().Contains(search) ||
                            e.PayrollGroupName_AR.Contains(search) ||
                            e.PayrollGroupName_EN.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<HRM_DEF_PayrollGroupDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdatePayRollGroup
    public class CreatePayRollGroup : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public HRM_DEF_PayrollGroupDto PayRollGroupDto { get; set; }
    }

    public class CreatePayRollGroupHandler : IRequestHandler<CreatePayRollGroup, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePayRollGroupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePayRollGroup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdatePayRollGroup method start----");



                var obj = request.PayRollGroupDto;


                HRM_DEF_PayrollGroup PayRollGroup = new();
                if (obj.PayrollGroupID > 0)
                    PayRollGroup = await _context.HRM_DEF_PayrollGroups.AsNoTracking().FirstOrDefaultAsync(e => e.PayrollGroupID == obj.PayrollGroupID);
                //else
                //{
                //    if (_context.HRM_DEF_PayrollGroups.Any(x => x.PayrollGroupID == obj.PayrollGroupID))
                //    {
                //        return -1;
                //    }
                //    PayRollGroup.PayrollGroupID = obj.PayrollGroupID.ToUpper();
                //}

               
                PayRollGroup.PayrollGroupName_EN = obj.PayrollGroupName_EN;
                PayRollGroup.PayrollGroupName_AR = obj.PayrollGroupName_AR;
                PayRollGroup.ProjectId = obj.ProjectId;
                PayRollGroup.SiteID = obj.SiteID;
                PayRollGroup.Remarks = obj.Remarks;
                PayRollGroup.StartPayRollDate= obj.StartPayRollDate;
                PayRollGroup.EndPayRollDate= obj.EndPayRollDate;
                PayRollGroup.BranchID= obj.BranchID;
                PayRollGroup.BusinessUnitID= obj.BusinessUnitID;
                PayRollGroup.DepartmentID= obj.DepartmentID;
                PayRollGroup.DivisionID= obj.DivisionID;
                PayRollGroup.CurrentPayRollMonth= obj.CurrentPayRollMonth;
                PayRollGroup.CurrentPayRollYear= obj.CurrentPayRollYear;
                PayRollGroup.CompanyID= obj.CompanyID;
                PayRollGroup.CountryID= obj.CountryID;
               


                if (obj.PayrollGroupID > 0)
                {
                    PayRollGroup.PayrollGroupID = obj.PayrollGroupID;
                    PayRollGroup.ModifiedDate = DateTime.Now;
                    PayRollGroup.ModifiedBy = request.User.UserId;
                    _context.HRM_DEF_PayrollGroups.Update(PayRollGroup);
                }
                else
                {

                    PayRollGroup.CreatedDate = DateTime.Now;
                    PayRollGroup.CreatedBy = request.User.UserId;
                    await _context.HRM_DEF_PayrollGroups.AddAsync(PayRollGroup);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdatePayRollGroup method Exit----");
                return PayRollGroup.PayrollGroupID;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdatePayRollGroup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    //#region GetPayRollGroupByPayRollGroupCode
    //public class GetPayRollGroupByPayRollGroupCode : IRequest<HRM_DEF_PayrollGroupDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public long PayRollGroupID { get; set; }
    //}

    //public class GetPayRollGroupByPayRollGroupCodeHandler : IRequestHandler<GetPayRollGroupByPayRollGroupCode, HRM_DEF_PayrollGroupDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetPayRollGroupByPayRollGroupCodeHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<HRM_DEF_PayrollGroupDto> Handle(GetPayRollGroupByPayRollGroupCode request, CancellationToken cancellationToken)
    //    {
        
    //        var PayRollGroup = await _context.HRM_DEF_PayrollGroups.AsNoTracking().ProjectTo<HRM_DEF_PayrollGroupDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.PayrollGroupID == request.PayRollGroupID);
          
    //        return PayRollGroup;
    //    }
    //}

    //#endregion

    #region GetPayRollGroupById
    public class GetPayRollGroupById : IRequest<HRM_DEF_PayrollGroupDto>
    {
        public UserIdentityDto User { get; set; }
        public long PayrollGroupID { get; set; }
    }

    public class GetPayRollGroupByIdHandler : IRequestHandler<GetPayRollGroupById, HRM_DEF_PayrollGroupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayRollGroupByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HRM_DEF_PayrollGroupDto> Handle(GetPayRollGroupById request, CancellationToken cancellationToken)
        {
         
            var PayRollGroup = await _context.HRM_DEF_PayrollGroups.AsNoTracking().ProjectTo<HRM_DEF_PayrollGroupDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.PayrollGroupID == request.PayrollGroupID);
            

            return PayRollGroup;
        }
    }

    #endregion

    #region GetSelectPayRollGroupList

    public class GetSelectPayRollGroupList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectPayRollGroupListHandler : IRequestHandler<GetSelectPayRollGroupList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectPayRollGroupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectPayRollGroupList request, CancellationToken cancellationToken)
        {

            var list = await _context.HRM_DEF_PayrollGroups.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.PayrollGroupName_EN, Value = e.PayrollGroupID.ToString(), TextTwo = e.PayrollGroupName_AR })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeletePayRollGroup
    public class DeletePayRollGroup : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int PayrollGroupID { get; set; }
    }

    public class DeletePayRollGroupQueryHandler : IRequestHandler<DeletePayRollGroup, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePayRollGroupQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePayRollGroup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeletePayRollGroup start----");

                if (request.PayrollGroupID > 0)
                {
                    var PayRollGroup = await _context.HRM_DEF_PayrollGroups.FirstOrDefaultAsync(e => e.PayrollGroupID == request.PayrollGroupID);
                    _context.Remove(PayRollGroup);

                    await _context.SaveChangesAsync();

                    return request.PayrollGroupID;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeletePayRollGroup");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
