using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using CIN.Domain.SchoolMgt;
using System.Threading;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetListByMobile
    public class GetAllParentMyGalleryByMobile : IRequest<List<TblParentMyGalleryDto>>
    {
        public UserIdentityDto User { get; set; }
        public string Mobile { get; set; }
    }

    public class GetAllParentMyGalleryByMobileHandler : IRequestHandler<GetAllParentMyGalleryByMobile,List<TblParentMyGalleryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetAllParentMyGalleryByMobileHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task <List<TblParentMyGalleryDto>> Handle(GetAllParentMyGalleryByMobile request,CancellationToken cancellationToken)
        {
            var list = await _context.ParentMyGallery.AsNoTracking().ProjectTo<TblParentMyGalleryDto>(_mapper.ConfigurationProvider).Where(e => e.RegisterMobile == request.Mobile).ToListAsync();
            return list;
        }
    }
    #endregion
}
