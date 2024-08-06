using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
using CIN.Domain.SchoolMgt;

namespace CIN.Application.SchoolMgtQuery
{
    #region Get School Branch List

    public class GetSchoolBranchList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectBranchCodeListHandler : IRequestHandler<GetSchoolBranchList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectBranchCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSchoolBranchList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSchoolBranchList method start----");
            var item = await _context.SchoolBranches.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode, TextTwo = e.BranchNameAr, })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSchoolBranchList method Ends----");
            return item;
        }
    }

    #endregion
}
