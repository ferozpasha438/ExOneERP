using AutoMapper;
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

    #region GetAll
    public class GetSysSchoolSectionsSectionList : IRequest<PaginatedList<TblSysSchoolSectionsSectionDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolSectionsSectionListHandler : IRequestHandler<GetSysSchoolSectionsSectionList, PaginatedList<TblSysSchoolSectionsSectionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolSectionsSectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolSectionsSectionDto>> Handle(GetSysSchoolSectionsSectionList request, CancellationToken cancellationToken)
        {

            
            var schoolSections = await _context.SchoolSectionsSection.AsNoTracking().ProjectTo<TblSysSchoolSectionsSectionDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return schoolSections;
        }


    }


    #endregion

    

    #region GetById

    public class GetSysSchoolSectionsSectionById : IRequest<TblSysSchoolSectionsSectionDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolSectionsSectionByIdHandler : IRequestHandler<GetSysSchoolSectionsSectionById, TblSysSchoolSectionsSectionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolSectionsSectionByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolSectionsSectionDto> Handle(GetSysSchoolSectionsSectionById request, CancellationToken cancellationToken)
        {

            TblSysSchoolSectionsSection obj = new();
            var SchoolSection = await _context.SchoolSectionsSection.AsNoTracking().ProjectTo<TblSysSchoolSectionsSectionDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return SchoolSection;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSysSchoolSections : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolSectionsSectionDto SchoolSectionsSectionDto { get; set; }
    }

    public class CreateUpdateSysSchoolSectionsHandler : IRequestHandler<CreateUpdateSysSchoolSections, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSysSchoolSectionsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSysSchoolSections request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSysSchoolSections method start----");

                var obj = request.SchoolSectionsSectionDto;


                TblSysSchoolSectionsSection SchoolSections = new();
                if (obj.Id > 0)
                    SchoolSections = await _context.SchoolSectionsSection.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                SchoolSections.Id = obj.Id;
                SchoolSections.SectionName = obj.SectionName;
                SchoolSections.SectionName2 = obj.SectionName2;
                SchoolSections.IsActive = obj.IsActive;
                SchoolSections.CreatedOn = DateTime.Now;
                SchoolSections.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SchoolSectionsSection.Update(SchoolSections);
                }
                else
                {
                    SchoolSections.SectionCode = obj.SectionCode.ToUpper();
                    await _context.SchoolSectionsSection.AddAsync(SchoolSections);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSysSchoolSections method Exit----");
                return SchoolSections.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSysSchoolSections Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolSectionsSection : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolSectionsSectionHandler : IRequestHandler<DeleteSchoolSectionsSection, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolSectionsSectionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolSectionsSection request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolSectionsSection start----");

                if (request.Id > 0)
                {
                    var section = await _context.SchoolSectionsSection.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(section);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolSectionsSection");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
