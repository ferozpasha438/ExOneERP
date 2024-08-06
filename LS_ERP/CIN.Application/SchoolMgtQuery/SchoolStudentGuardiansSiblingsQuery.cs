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
    #region GetAll
    public class GetGuardiansSiblingsList : IRequest<PaginatedList<TblDefStudentGuardiansSiblingsDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetGuardiansSiblingsListHandler : IRequestHandler<GetGuardiansSiblingsList, PaginatedList<TblDefStudentGuardiansSiblingsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGuardiansSiblingsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefStudentGuardiansSiblingsDto>> Handle(GetGuardiansSiblingsList request, CancellationToken cancellationToken)
        {

            var studentGaurdianSiblings = await _context.DefStudentGuardiansSiblings.AsNoTracking().ProjectTo<TblDefStudentGuardiansSiblingsDto>(_mapper.ConfigurationProvider)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return studentGaurdianSiblings;
        }


    }
    #endregion

    #region GetById
    public class GetGuardiansSiblingsById : IRequest<TblDefStudentGuardiansSiblingsDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id  { get; set; }
    }
    public class GetGuardiansSiblingsByIdHandler : IRequestHandler<GetGuardiansSiblingsById ,TblDefStudentGuardiansSiblingsDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetGuardiansSiblingsByIdHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentGuardiansSiblingsDto> Handle(GetGuardiansSiblingsById request, CancellationToken cancellationToken)
        {
            var gaurdianSibling = await _context.DefStudentGuardiansSiblings.AsNoTracking().ProjectTo<TblDefStudentGuardiansSiblingsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return gaurdianSibling;
        }
    }

    #endregion

    #region Create_And_Update
    public class CreateUpdateGuardiansSiblings : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentGuardiansSiblingsDto StudentGuardiansSiblingsDto { get; set; }
    }
    public class CreateUpdateGuardiansSiblingsHandler : IRequestHandler<CreateUpdateGuardiansSiblings, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateUpdateGuardiansSiblingsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateUpdateGuardiansSiblings request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update method start----");

                var obj = request.StudentGuardiansSiblingsDto;

                TblDefStudentGuardiansSiblings GuardiansSiblings = new();
                if (obj.Id > 0)
                    GuardiansSiblings = await _context.DefStudentGuardiansSiblings.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                GuardiansSiblings.Id = obj.Id;
                GuardiansSiblings.StuAdmNum = obj.StuAdmNum;
                GuardiansSiblings.RelationType = obj.RelationType;
                GuardiansSiblings.Name = obj.Name;
                GuardiansSiblings.Occupation = obj.Occupation;
                GuardiansSiblings.Deisgnation = obj.Deisgnation;
                GuardiansSiblings.Mobile1 = obj.Mobile1;
                GuardiansSiblings.email = obj.email;
                GuardiansSiblings.Remarks = obj.Remarks;

                if (obj.Id > 0)
                {
                    _context.DefStudentGuardiansSiblings.Update(GuardiansSiblings);
                }
                else
                {
                    await _context.DefStudentGuardiansSiblings.AddAsync(GuardiansSiblings);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update method Exit----");
                return GuardiansSiblings.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }

    #endregion

    #region Delete
    public class DeleteGaurdianSiblings : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }
    public class DeleteGaurdianSiblingsHandler : IRequestHandler<DeleteGaurdianSiblings, int>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public DeleteGaurdianSiblingsHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       public async Task<int>Handle(DeleteGaurdianSiblings request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteGaurdianSiblings start----");

                if (request.Id > 0)
                {
                    var gaurdianSiblings = await _context.DefStudentGuardiansSiblings.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(gaurdianSiblings);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Gaurdian Siblings");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }

    }

    #endregion


    #region GetByParentProfile
    public class GetParentProfile : IRequest<List<TblDefStudentGuardiansSiblingsDto>>
    {
        public UserIdentityDto User { get; set; }
        public string Mobile { get; set; }
    }
    public class GetParentProfileHandler : IRequestHandler<GetParentProfile, List<TblDefStudentGuardiansSiblingsDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetParentProfileHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblDefStudentGuardiansSiblingsDto>> Handle(GetParentProfile request, CancellationToken cancellationToken)
        {
            var ParentDetails = await _context.DefStudentGuardiansSiblings.AsNoTracking().ProjectTo<TblDefStudentGuardiansSiblingsDto>(_mapper.ConfigurationProvider).Where(e => e.Mobile1 == request.Mobile && (e.RelationType == "father" || e.RelationType == "mother")).ToListAsync();
            return ParentDetails;
           
        }
    }

    #endregion


    #region GetGuardiansSiblingsByStuAdmNum
    public class GetGuardiansSiblingsByStuAdmNum : IRequest<PaginatedList<TblDefStudentGuardiansSiblingsDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetGuardiansSiblingsByStuAdmNumHandler : IRequestHandler<GetGuardiansSiblingsByStuAdmNum, PaginatedList<TblDefStudentGuardiansSiblingsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGuardiansSiblingsByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefStudentGuardiansSiblingsDto>> Handle(GetGuardiansSiblingsByStuAdmNum request, CancellationToken cancellationToken)
        {

            var studentGaurdianSiblings = await _context.DefStudentGuardiansSiblings.AsNoTracking().ProjectTo<TblDefStudentGuardiansSiblingsDto>(_mapper.ConfigurationProvider).Where(x=>x.StuAdmNum==request.Input.StuAdmNum)
                                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return studentGaurdianSiblings;
        }


    }
    #endregion







}
