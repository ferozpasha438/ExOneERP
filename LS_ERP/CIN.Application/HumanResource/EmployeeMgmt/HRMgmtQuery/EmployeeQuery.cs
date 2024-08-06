using AutoMapper;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetEmployeeSelectListItem
    public class GetEmployeeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetEmployeeSelectListItemHandler : IRequestHandler<GetEmployeeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetEmployeeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.PersonalInformation.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { IntValue = e.Id, Text = isArab ? string.Concat(e.FirstNameAr, " ", e.LastNameAr) : string.Concat(e.FirstNameEn, " ", e.LastNameEn), Value = e.EmployeeNumber })
                  .ToListAsync(cancellationToken);

            return list;

        }
    }
    #endregion
}
