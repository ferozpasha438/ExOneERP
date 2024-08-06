//using AutoMapper;
//using CIN.Application.Common;
//using CIN.Application.SchoolMgtDto;
//using CIN.DB;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper.QueryableExtensions;
//using System.Linq.Dynamic.Core;
//using CIN.Domain.SchoolMgt;

//namespace CIN.Application.SchoolMgtQuery
//{
//    #region GetTestemployeeList
//    public class GetStudentDetailsList : IRequest<List<TblStudentDetailsDto>>
//    {
//        public UserIdentityDto User { get; set; }
//        public TblStudentDetailsDto StudentDetailsDto { get; set; }

//    }

//    public class GetStudentDetailsListHandler : IRequestHandler<GetStudentDetailsList, List<TblStudentDetailsDto>>
//    {
//        private readonly CINDBOneContext _context;
//        private readonly IMapper _mapper;
//        public GetStudentDetailsListHandler(CINDBOneContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }
//        public async Task<List<TblStudentDetailsDto>> Handle(GetStudentDetailsList request, CancellationToken cancellationToken)
//        {
//            TblStudentDetailsDto obj = new();
//            var studentDetails = await _context.StudentDetails.AsNoTracking().ProjectTo<TblStudentDetailsDto>(_mapper.ConfigurationProvider).ToListAsync();

//            return studentDetails;
//        }
//    }

//    #endregion
//}
