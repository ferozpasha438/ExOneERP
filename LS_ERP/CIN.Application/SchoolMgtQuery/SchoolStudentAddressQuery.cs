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
    public class GetDefStudentAddressList : IRequest<PaginatedList<TblDefStudentAddressDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetDefStudentAddressListHandler : IRequestHandler<GetDefStudentAddressList, PaginatedList<TblDefStudentAddressDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDefStudentAddressListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefStudentAddressDto>> Handle(GetDefStudentAddressList request, CancellationToken cancellationToken)
        {


            var studentAddressList = await _context.DefStudentAddress.AsNoTracking().ProjectTo<TblDefStudentAddressDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return studentAddressList;
        }


    }


    #endregion

    #region GetById

    public class GetDefStudentAddressById : IRequest<List<TblDefStudentAddressDto>>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDefStudentAddressByIdHandler : IRequestHandler<GetDefStudentAddressById, List<TblDefStudentAddressDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDefStudentAddressByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblDefStudentAddressDto>> Handle(GetDefStudentAddressById request, CancellationToken cancellationToken)
        {
            List<TblDefStudentAddressDto> list = new List<TblDefStudentAddressDto>();
            try
            {
                string criteria = "x=>x.City=='hyd'"; 
                //list = await _context.DefStudentAddress.FromSqlRaw("select Id,StuAdmNum,PAddress1,City,ZipCode,Country,Phone1,Mobile1 from tblDefStudentAddress where " + criteria)
                //                               .Select(x=>
                //                                  new TblDefStudentAddressDto { 
                //                                      City=x.City,
                //                                      Country=x.Country,
                //                                      Id=x.Id,
                //                                      Mobile1=x.Mobile1,
                //                                      PAddress1=x.PAddress1,
                //                                      Phone1=x.Phone1,
                //                                      StuAdmNum=x.StuAdmNum,
                //                                      ZipCode=x.ZipCode
                //                                  }).ToListAsync();

                 list = await _context.DefStudentAddress.AsNoTracking().ProjectTo<TblDefStudentAddressDto>(_mapper.ConfigurationProvider).Where(x=>x.City=="hyd").ToListAsync();

                
            }
            catch (Exception ex)
            {

                throw;
            }
           // return list;
            return list;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update

    public class CreateUpdateDefStudentAddress : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentAddressDto StudentAddressDto { get; set; }
    }
    public class CreateUpdateDefStudentAddressHandler : IRequestHandler<CreateUpdateDefStudentAddress, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateDefStudentAddressHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateUpdateDefStudentAddress request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateDefStudentAddress method start----");

                var obj = request.StudentAddressDto;

                TblDefStudentAddress StudentAddress = new();
                if (obj.Id > 0)
                    StudentAddress = await _context.DefStudentAddress.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                StudentAddress.Id = obj.Id;
                StudentAddress.StuAdmNum = obj.StuAdmNum;
                StudentAddress.BuildingName = obj.BuildingName;
                StudentAddress.PAddress1 = obj.PAddress1;
                StudentAddress.City = obj.City;
                StudentAddress.ZipCode = obj.ZipCode;
                StudentAddress.Country = obj.Country;
                StudentAddress.Phone1 = obj.Phone1;
                StudentAddress.Mobile1 = obj.Mobile1;

                if (obj.Id > 0)
                {
                    _context.DefStudentAddress.Update(StudentAddress);
                }
                else
                {
                    await _context.DefStudentAddress.AddAsync(StudentAddress);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateDefStudentAddress method Exit----");
                return StudentAddress.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateDefStudentAddress Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteDefStudentAddress : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDefStudentAddressHandler : IRequestHandler<DeleteDefStudentAddress, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDefStudentAddressHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDefStudentAddress request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Address start----");

                if (request.Id > 0)
                {
                    var subject = await _context.DefStudentAddress.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(subject);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteDefStudentAddress");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

    #region GetStudentAddressList
    public class GetStudentAddressList : IRequest<PaginatedList<TblDefStudentAddressDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetStudentAddressListHandler : IRequestHandler<GetStudentAddressList, PaginatedList<TblDefStudentAddressDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentAddressListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefStudentAddressDto>> Handle(GetStudentAddressList request, CancellationToken cancellationToken)
        {


            var studentAddressList = await _context.DefStudentAddress.AsNoTracking().ProjectTo<TblDefStudentAddressDto>(_mapper.ConfigurationProvider).Where(x=>x.StuAdmNum==request.Input.StuAdmNum)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return studentAddressList;
        }


    }


    #endregion
}
