using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
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
    #region GetNewsList

    public class GetSchoolNewsList : IRequest<TblSchoolMeadiaNewsDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime NewsDate { get; set; }
    }

    public class GetSchoolNewsListHandler : IRequestHandler<GetSchoolNewsList, TblSchoolMeadiaNewsDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetSchoolNewsListHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<TblSchoolMeadiaNewsDto> Handle(GetSchoolNewsList requset,CancellationToken cancellationToken)
        {

            return new TblSchoolMeadiaNewsDto
            {
                SchoolNewsList = await _context.SysSchoolNews.AsNoTracking().ProjectTo<TblSysSchoolNewsDto>(_mapper.ConfigurationProvider).Where(e => e.NewsDate == requset.NewsDate).ToListAsync(),

                SchoolNewsDetailList = await _context.SysSchoolNewsMedia
                 .Join(_context.SysSchoolNews, NewsMedia => NewsMedia.NewId, News => News.NewId,
                      (NewsMedia, News) => new TblSysSchoolNewsMediaLibDto
                      {
                          Id = NewsMedia.Id,
                          NewId = NewsMedia.NewId,
                          Mediapath = NewsMedia.Mediapath,
                          MediaType = NewsMedia.MediaType,
                          MediaNotes = NewsMedia.MediaNotes,
                          IsActive = NewsMedia.IsActive
                      }
                 ).ToListAsync()


                };

                //return new TblSchoolMeadiaNewsDto
                //{
                //var q = (from n in _context.SysSchoolNews
                //         join m in _context.SysSchoolNewsMedia on n.NewId equals m.NewId
                //         orderby n.NewId
                //         select new
                //         {
                //             n.NewId,
                //             n.Topic,
                //             n.Headlines,
                //             n.NewsDetails,
                //             n.NewsDate,
                //             n.NewTumbnailImagePath,
                //             n.PostedBy,
                //             n.IsApproved,
                //             n.ApprovedBy,
                //             n.ApproveDate,
                //             m.Id,
                //             m.Mediapath,
                //             m.MediaType,
                //             m.MediaNotes,
                //             m.IsActive
                //         }).ToList();

            }
    }
    #endregion
}
