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

    public class GetAllStudentPrevEducationList : IRequest<PaginatedList<TblDefStudentPrevEducationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAllStudentPrevEducationListHandler : IRequestHandler<GetAllStudentPrevEducationList, PaginatedList<TblDefStudentPrevEducationDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetAllStudentPrevEducationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblDefStudentPrevEducationDto>> Handle(GetAllStudentPrevEducationList request, CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentPrevEducation.AsNoTracking().ProjectTo<TblDefStudentPrevEducationDto>(_mapper.ConfigurationProvider)
                                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region GetById
    public class GetStudentPrevEducationById : IRequest<TblDefStudentPrevEducationDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class GetStudentPrevEducationByIdHandler : IRequestHandler<GetStudentPrevEducationById, TblDefStudentPrevEducationDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetStudentPrevEducationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblDefStudentPrevEducationDto> Handle(GetStudentPrevEducationById request, CancellationToken cancellationToken)
        {
            var studentPrevEducation = await _context.DefStudentPrevEducation.AsNoTracking().ProjectTo<TblDefStudentPrevEducationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return studentPrevEducation;
        }
    }
    #endregion

    #region CreateAndUpdate
    public class CreateUpdateStudentPrevEducation : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentPrevEducationDto Input { get; set; }
    }

    public class CreateUpdateStudentPrevEducationHandler : IRequestHandler<CreateUpdateStudentPrevEducation, int>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public CreateUpdateStudentPrevEducationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentPrevEducation request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdate Student PrevEducation method start----");
                var obj = request.Input;

                TblDefStudentPrevEducation studentPrevEducation = new();
                if (obj.Id > 0)
                    studentPrevEducation = await _context.DefStudentPrevEducation.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                studentPrevEducation.Id = obj.Id;
                studentPrevEducation.StuAdmNum = obj.StuAdmNum;
                studentPrevEducation.NameOfInstitute = obj.NameOfInstitute;
                studentPrevEducation.ClassStudied = obj.ClassStudied;
                studentPrevEducation.LanguageMedium = obj.LanguageMedium;
                studentPrevEducation.BoardName = obj.BoardName;
                studentPrevEducation.PassPercentage = obj.PassPercentage;
                studentPrevEducation.YearofPassing = obj.YearofPassing;

                if (obj.Id > 0)
                {
                    _context.DefStudentPrevEducation.Update(studentPrevEducation);
                }
                else
                {
                    await _context.DefStudentPrevEducation.AddAsync(studentPrevEducation);
                }
                await _context.SaveChangesAsync();
                return studentPrevEducation.Id;

            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update StudentPrevEducation Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;

            }

        }
    }
    #endregion

    #region Delete
    public class DeleteStudentPrevEducation : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class DeleteStudentPrevEducationHandler : IRequestHandler<DeleteStudentPrevEducation, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentPrevEducationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<int> Handle(DeleteStudentPrevEducation request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Prev Education start----");
                if (request.Id > 0)
                {
                    var studentPreveducation = await _context.DefStudentPrevEducation.AsNoTracking().ProjectTo<TblDefStudentPrevEducationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(studentPreveducation);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Student Prev Education");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }

    }

    #endregion

    #region GetPrevEducationListByStuAdmNum

    public class GetPrevEducationListByStuAdmNum : IRequest<PaginatedList<TblDefStudentPrevEducationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPrevEducationListByStuAdmNumHandler : IRequestHandler<GetPrevEducationListByStuAdmNum, PaginatedList<TblDefStudentPrevEducationDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetPrevEducationListByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblDefStudentPrevEducationDto>> Handle(GetPrevEducationListByStuAdmNum request, CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentPrevEducation.AsNoTracking().ProjectTo<TblDefStudentPrevEducationDto>(_mapper.ConfigurationProvider).Where(x => x.StuAdmNum == request.Input.StuAdmNum)
                                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

}
