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

    

    

    #region GetUserByUserId
    public class GetUserByUserId : IRequest<TblErpSysLogin>
    {
        public UserIdentityDto User { get; set; }
        public int Userid { get; set; }
    }

    public class GetUserByUserIdHandler : IRequestHandler<GetUserByUserId, TblErpSysLogin>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysLogin> Handle(GetUserByUserId request, CancellationToken cancellationToken)
        {
            TblErpSysLogin obj = new();
            var User = await _context.SystemLogins.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Userid);
                return obj;
       
        }
    }

    #endregion






    #region GetUserSelectionList
    public class GetUserSelectionList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Userid { get; set; }
    }

    public class GetUserSelectionListHandler : IRequestHandler<GetUserSelectionList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserSelectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUserSelectionList request, CancellationToken cancellationToken)
        {
            
           var Users = await _context.SystemLogins.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.UserName, Value = e.Id.ToString(),TextTwo=e.PrimaryBranch })
                  .ToListAsync(cancellationToken);
            return Users;

        }
    }

    #endregion








}
