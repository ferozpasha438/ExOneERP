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
using System.Globalization;

namespace CIN.Application.OperationsMgtQuery.Shared
{


    #region GetIsHoliday   Common Holidays for All sites and projects

    public class IsHoliday : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public DateTime Date { get; set; }
    }

    public class IsHolidayHandler : IRequestHandler<IsHoliday, bool>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public IsHolidayHandler(DMCContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(IsHoliday request, CancellationToken cancellationToken)
        {
            request.Date = Convert.ToDateTime(request.Date,CultureInfo.InvariantCulture);
            return await _context.HRM_DEF_Holidays.AnyAsync(e=>e.HolidayDate.Value==request.Date);
            
        }
    }
    #endregion











    

}
